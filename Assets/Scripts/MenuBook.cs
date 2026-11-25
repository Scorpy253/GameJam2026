using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class MenuShop : MonoBehaviour
{
    [Header("Icon Loading Settings")]
    [SerializeField] private string iconFolderPath = "SpellIcons"; // Loads Resources/<iconFolderPath>/<spellName>
    [Header("1A. Spell List Page (Dynamic Spawn - All Spells)")]
    [SerializeField] private Transform spellListContainer;         // Has Horizontal/Grid Layout Group
    [SerializeField] private SpellSlotUI spellSlotPrefab;          // Your pre-wired prefab
    [Header("1B. Equipped Spells Panel (15 Pre-placed Slots on Page 1)")]
    [Tooltip("Children 0-2: Fire (1-3), 3-5: Lightning (4-6), 6-8: Water (7-9), 9-11: Earth (10-12), 12-14: Wind (13-15)")]
    [SerializeField] private Transform equippedSpellsContainer;    // Parent holding the 15 pre-placed prefabs
    [SerializeField] private Sprite lockedEquipSlotSprite;         // Optional lock sprite for slots not yet unlocked on Page 3
    [Header("2. Skill Tree Page (Element Parent Folders & Scaling Cost)")]
    [SerializeField] private Transform fireTreeParent;               // Children ordered root (0) -> top (N)
    [SerializeField] private Transform waterTreeParent;
    [SerializeField] private Transform earthTreeParent;
    [SerializeField] private Transform windTreeParent;
    [SerializeField] private Transform lightTreeParent;
    [SerializeField] private int baseSpellUnlockCost = 1;          // Starting element coin cost for the 1st tree unlock
    [SerializeField] private float spellCostMultiplier = 1.5f;     // Multiplies per already-unlocked spell in that tree
    [Header("3. Upgrades Page (8 Bars x 9 Segments & Scaling Costs)")]
    [SerializeField] private Transform atkUpParent;                  // AtkUp (Level1..Level9)
    [SerializeField] private Transform spdUpParent;                  // SpdUp (Level1..Level9)
    [SerializeField] private Transform hpUpParent;                   // HpUp  (Level1..Level9)
    [SerializeField] private Transform fireSlotsParent;              // FireSlots (Level1..Level9 -> 3 pips per slot)
    [SerializeField] private Transform iceSlotsParent;               // IceSlots  (Level1..Level9 -> 3 pips per slot)
    [SerializeField] private Transform earthSlotsParent;             // EarthSlots (Level1..Level9 -> 3 pips per slot)
    [SerializeField] private Transform windSlotsParent;              // WindSlots (Level1..Level9 -> 3 pips per slot)
    [SerializeField] private Transform lightningSlotsParent;         // LightningSlots (Level1..Level9 -> 3 pips per slot)
    [SerializeField] private int baseStatUpgradeCost = 10;         // Starting gold cost at level 0
    [SerializeField] private float statCostMultiplier = 1.35f;     // Exponential growth per stat level (0..8)
    [SerializeField] private int baseSlotUpgradeCost = 2;          // Starting element coin cost for slot 2
    [SerializeField] private float slotCostMultiplier = 2.5f;      // Exponential growth per extra slot unlocked
    [SerializeField] private Sprite litSegmentSprite;                // Lit sprite
    [SerializeField] private Sprite unlitSegmentSprite;              // Unlit sprite
    [Header("Spell Information Display (Floating Tooltip)")]
    [SerializeField] private RectTransform infoTooltipPanel;         // Drag your SpellInfoTooltip RectTransform here
    [SerializeField] private float tooltipVerticalOffset = 12f;      // Gap between slot edge and tooltip
    [SerializeField] private Image infoIcon;
    [SerializeField] private TextMeshProUGUI infoNameText;
    [SerializeField] private TextMeshProUGUI infoDamageText;
    [SerializeField] private TextMeshProUGUI infoCooldownText;
    [SerializeField] private TextMeshProUGUI infoSequenceText;
    [SerializeField] private TextMeshProUGUI infoDescriptionText;
    [SerializeField] private Button unequipButton;                   // Shown only when viewing an equipped spell

    // Element order matching the 15 hierarchy slots (every 3 children = 1 element, slots 1..15)
    private static readonly SpellsList.Element[] EquipElementOrder = new SpellsList.Element[]
    {
        SpellsList.Element.Fire,      // Children 0..2   -> Slots 1..3
        SpellsList.Element.Lightning, // Children 3..5   -> Slots 4..6
        SpellsList.Element.Water,     // Children 6..8   -> Slots 7..9
        SpellsList.Element.Earth,     // Children 9..11  -> Slots 10..12
        SpellsList.Element.Wind       // Children 12..14 -> Slots 13..15
    };

    private SaveData saveData;
    private List<SpellsList> allSpells = new List<SpellsList>();
    private SpellSlotUI selectedSlot;

    // Equip-slot selection state
    private int pendingEquipSlotIndex = -1; // 0..14 when waiting for player to pick a spell, -1 otherwise
    private SpellsList.Element pendingElementFilter;

    private void Awake()
    {
        LoadAllSpellClasses();
        Debug.Log(Application.persistentDataPath);
        if (unequipButton != null)
        {
            unequipButton.onClick.RemoveAllListeners();
            unequipButton.onClick.AddListener(UnequipDisplayedSpell);
            unequipButton.gameObject.SetActive(false);
        }
    }

    private void LoadAllSpellClasses()
    {
        allSpells = new List<SpellsList>();

        var spellTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(SpellsList)));

        foreach (Type type in spellTypes)
        {
            SpellsList spellInstance = (SpellsList)Activator.CreateInstance(type);
            allSpells.Add(spellInstance);
        }
    }

    private void OnEnable()
    {
        CancelEquipSelection();
        HideSpellInfo();
        RefreshAll();
    }

    private void Update()
    {
        bool tooltipOpen = (infoTooltipPanel != null && infoTooltipPanel.gameObject.activeSelf);
        if (pendingEquipSlotIndex == -1 && !tooltipOpen) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleOutsidePointerPress(Mouse.current.position.ReadValue());
            return;
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            HandleOutsidePointerPress(Touchscreen.current.primaryTouch.position.ReadValue());
        }
    }

    private void HandleOutsidePointerPress(Vector2 screenPosition)
    {
        if (EventSystem.current == null) return;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        bool clickedAnySpellSlot = false;
        bool clickedInsideTooltip = false;
        bool clickedValidEquipChoice = false;

        foreach (RaycastResult hit in results)
        {
            Transform hitTransform = hit.gameObject.transform;

            if (infoTooltipPanel != null && hitTransform.IsChildOf(infoTooltipPanel))
                clickedInsideTooltip = true;

            SpellSlotUI hitSlot = hit.gameObject.GetComponentInParent<SpellSlotUI>();
            if (hitSlot != null)
                clickedAnySpellSlot = true;

            if (equippedSpellsContainer != null && hitTransform.IsChildOf(equippedSpellsContainer))
                clickedValidEquipChoice = true;

            if (hitSlot != null &&
                spellListContainer != null &&
                hitSlot.transform.IsChildOf(spellListContainer) &&
                hitSlot.spellData != null &&
                hitSlot.spellData.element == pendingElementFilter &&
                IsSpellUnlocked(hitSlot.spellData.spellName))
            {
                clickedValidEquipChoice = true;
            }
        }

        // Cancel equip-slot dimming if clicking outside valid equip targets
        if (pendingEquipSlotIndex != -1 && !clickedValidEquipChoice)
            CancelEquipSelection();

        // Hide the floating tooltip if clicking anywhere other than a spell slot or the tooltip itself
        if (!clickedAnySpellSlot && !clickedInsideTooltip)
            HideSpellInfo();
    }

    public void RefreshAll()
    {
        if (allSpells == null || allSpells.Count == 0)
            LoadAllSpellClasses();

        saveData = SaveSystem.Load();
        if (saveData == null) saveData = new SaveData();

        LoadSpellList();
        LoadEquippedSlotsFromSave();
        LoadSkillTree();
        LoadUpgradesPage();
        SelectInitialSpellForDisplay();
    }

    private Sprite GetSpellIcon(string spellName)
    {
        string path = string.IsNullOrEmpty(iconFolderPath) ? spellName : $"{iconFolderPath}/{spellName}";
        return Resources.Load<Sprite>(path);
    }

    public bool IsSpellUnlocked(string spellName)
    {
        if (saveData == null || saveData.unlockedSpellNames == null) return false;
        return saveData.unlockedSpellNames.Contains(spellName);
    }

    public bool IsSpellEquipped(string spellName)
    {
        if (saveData == null || saveData.equippedSpells == null || string.IsNullOrEmpty(spellName)) return false;
        return saveData.equippedSpells.Any(e => e != null && e.SpellName == spellName);
    }

    // =========================================================
    // DYNAMIC COST FORMULAS (Calculated purely from SaveData)
    // =========================================================
    public int GetNextSpellUnlockCost(SpellsList.Element element)
    {
        if (baseSpellUnlockCost <= 0) return 0;
        if (saveData == null) saveData = SaveSystem.Load() ?? new SaveData();

        // Count spells already unlocked in this element, excluding the 1 default starter spell
        int totalUnlockedInElement = allSpells.Count(s => s.element == element && IsSpellUnlocked(s.spellName));
        int treeUnlocksPurchased = Mathf.Max(0, totalUnlockedInElement - 1);

        return Mathf.Max(1, Mathf.RoundToInt(baseSpellUnlockCost * Mathf.Pow(spellCostMultiplier, treeUnlocksPurchased)));
    }

    public int GetNextDamageUpgradeCost()
    {
        if (saveData == null) saveData = SaveSystem.Load() ?? new SaveData();
        return GetNextStatUpgradeCost(saveData.dmgLevel);
    }

    public int GetNextSpeedUpgradeCost()
    {
        if (saveData == null) saveData = SaveSystem.Load() ?? new SaveData();
        return GetNextStatUpgradeCost(saveData.speedLevel);
    }

    public int GetNextHpUpgradeCost()
    {
        if (saveData == null) saveData = SaveSystem.Load() ?? new SaveData();
        return GetNextStatUpgradeCost(saveData.hpLevel);
    }

    private int GetNextStatUpgradeCost(int currentLevel)
    {
        if (baseStatUpgradeCost <= 0) return 0;
        int clampedLevel = Mathf.Max(0, currentLevel);
        return Mathf.Max(1, Mathf.RoundToInt(baseStatUpgradeCost * Mathf.Pow(statCostMultiplier, clampedLevel)));
    }

    public int GetNextSlotUpgradeCost(SpellsList.Element element)
    {
        if (baseSlotUpgradeCost <= 0) return 0;
        if (saveData == null) saveData = SaveSystem.Load() ?? new SaveData();

        // Default is 1 slot unlocked; 0 extra purchased -> slot 2, 1 extra purchased -> slot 3
        int extraSlotsPurchased = Mathf.Max(0, GetUnlockedSlotCountForElement(element) - 1);
        return Mathf.Max(1, Mathf.RoundToInt(baseSlotUpgradeCost * Mathf.Pow(slotCostMultiplier, extraSlotsPurchased)));
    }

    // ---------------------------------------------------------
    // 1A. SPELL LIST: Spawns EVERY spell side-by-side
    // ---------------------------------------------------------
    public void LoadSpellList()
    {
        if (spellListContainer == null || spellSlotPrefab == null) return;

        foreach (Transform child in spellListContainer)
            Destroy(child.gameObject);

        foreach (SpellsList spell in allSpells)
        {
            SpellSlotUI slot = Instantiate(spellSlotPrefab, spellListContainer, false);
            slot.SetSpell(spell, GetSpellIcon(spell.spellName), this);
        }

        UpdateSpellListVisualStates();
    }

    private void UpdateSpellListVisualStates()
    {
        if (spellListContainer == null) return;

        bool isFiltering = (pendingEquipSlotIndex != -1);

        for (int i = 0; i < spellListContainer.childCount; i++)
        {
            SpellSlotUI slot = spellListContainer.GetChild(i).GetComponent<SpellSlotUI>();
            if (slot == null || slot.spellData == null) continue;

            bool unlocked = IsSpellUnlocked(slot.spellData.spellName);

            if (isFiltering)
            {
                bool matchesElement = (slot.spellData.element == pendingElementFilter) && unlocked;
                SetSlotLockState(slot, !matchesElement);
                SetSlotVisualTint(slot, matchesElement ? Color.white : new Color(0.25f, 0.25f, 0.25f, 0.45f));
            }
            else
            {
                SetSlotLockState(slot, false);
                SetSlotVisualTint(slot, unlocked ? Color.white : new Color(0.45f, 0.45f, 0.45f, 1f));
            }
        }
    }

    // ---------------------------------------------------------
    // 1B. EQUIPPED PANEL:
    //     - Unlocked & Equipped   -> Shows spell icon (alpha = 1)
    //     - Unlocked & Unequipped -> Transparent icon (alpha = 0), clickable to equip
    //     - Locked (beyond element's unlocked slot count) -> Locked & non-interactable
    // ---------------------------------------------------------
    public void LoadEquippedSlotsFromSave()
    {
        if (equippedSpellsContainer == null) return;

        if (saveData.equippedSpells == null)
            saveData.equippedSpells = new List<EquippedSpells>();

        int childCount = equippedSpellsContainer.childCount;

        for (int i = 0; i < childCount; i++)
        {
            SpellSlotUI slotUI = equippedSpellsContainer.GetChild(i).GetComponent<SpellSlotUI>();
            if (slotUI == null) continue;

            int elementGroupIndex = i / 3; // 0..4
            int slotInElement = i % 3;     // 0, 1, or 2
            int globalSlotNumber = i + 1;  // 1..15

            if (elementGroupIndex >= EquipElementOrder.Length)
            {
                slotUI.gameObject.SetActive(false);
                continue;
            }

            slotUI.gameObject.SetActive(true);
            SpellsList.Element groupElement = EquipElementOrder[elementGroupIndex];
            int unlockedForElement = Mathf.Clamp(GetUnlockedSlotCountForElement(groupElement), 1, 3);
            bool isSlotLocked = slotInElement >= unlockedForElement;

            if (isSlotLocked)
            {
                slotUI.SetSpell(null, lockedEquipSlotSprite, this);
                SetSlotLockState(slotUI, true);
                SetSlotIconAlpha(slotUI, lockedEquipSlotSprite != null ? 1f : 0f);
                continue;
            }

            SetSlotLockState(slotUI, false);

            EquippedSpells savedEntry = saveData.equippedSpells.FirstOrDefault(e => e != null && e.SpellSlot == globalSlotNumber);
            SpellsList matchedSpell = (savedEntry != null && !string.IsNullOrEmpty(savedEntry.SpellName))
                ? allSpells.FirstOrDefault(s => s.spellName == savedEntry.SpellName)
                : null;

            if (matchedSpell != null)
            {
                slotUI.SetSpell(matchedSpell, GetSpellIcon(matchedSpell.spellName), this);
                SetSlotIconAlpha(slotUI, 1f);
            }
            else
            {
                slotUI.SetSpell(null, null, this);
                SetSlotIconAlpha(slotUI, 0f);
            }
        }
    }

    public void OnSlotPressed(SpellSlotUI pressedSlot)
    {
        if (pressedSlot == null) return;

        // CASE 1: Player clicked one of the 15 Equipped Slots on Page 1
        if (equippedSpellsContainer != null && pressedSlot.transform.IsChildOf(equippedSpellsContainer))
        {
            int childIndex = pressedSlot.transform.GetSiblingIndex();
            int elementGroupIndex = childIndex / 3;
            int slotInElement = childIndex % 3;

            if (elementGroupIndex < 0 || elementGroupIndex >= EquipElementOrder.Length) return;

            SpellsList.Element slotElement = EquipElementOrder[elementGroupIndex];
            int unlockedForElement = Mathf.Clamp(GetUnlockedSlotCountForElement(slotElement), 1, 3);

            if (slotInElement >= unlockedForElement) return;

            if (pressedSlot.spellData != null)
            {
                DisplaySpellInfo(pressedSlot);
            }
            else
            {
                HideSpellInfo();
            }

            BeginEquipSelectionForSlot(childIndex, slotElement);
            return;
        }

        // CASE 2: Player is in Equip Selection Mode and clicked a matching unlocked spell in the Spell List
        if (pendingEquipSlotIndex != -1 &&
            spellListContainer != null &&
            pressedSlot.transform.IsChildOf(spellListContainer) &&
            pressedSlot.spellData != null)
        {
            SpellsList chosenSpell = pressedSlot.spellData;
            if (chosenSpell.element == pendingElementFilter && IsSpellUnlocked(chosenSpell.spellName))
            {
                EquipSpellIntoSpecificSlot(chosenSpell, pendingEquipSlotIndex);
                CancelEquipSelection();
                DisplaySpellInfo(pressedSlot);
                return;
            }
        }

        // CASE 3: Normal inspection click (Spell List or Skill Tree)
        if (pendingEquipSlotIndex != -1)
        {
            CancelEquipSelection();
        }

        if (pressedSlot.spellData != null)
        {
            DisplaySpellInfo(pressedSlot);
        }
    }

    private void BeginEquipSelectionForSlot(int equipChildIndex, SpellsList.Element element)
    {
        pendingEquipSlotIndex = equipChildIndex;
        pendingElementFilter = element;
        UpdateSpellListVisualStates();
    }

    public void CancelEquipSelection()
    {
        pendingEquipSlotIndex = -1;
        UpdateSpellListVisualStates();
    }

    private void EquipSpellIntoSpecificSlot(SpellsList spell, int targetChildIndex)
    {
        if (spell == null || equippedSpellsContainer == null) return;
        if (targetChildIndex < 0 || targetChildIndex >= equippedSpellsContainer.childCount) return;

        int globalSlotNumber = targetChildIndex + 1;

        for (int i = 0; i < equippedSpellsContainer.childCount; i++)
        {
            SpellSlotUI existingUI = equippedSpellsContainer.GetChild(i).GetComponent<SpellSlotUI>();
            if (existingUI != null && existingUI.spellData != null && existingUI.spellData.spellName == spell.spellName)
            {
                existingUI.SetSpell(null, null, this);
                SetSlotIconAlpha(existingUI, 0f);
            }
        }

        SpellSlotUI targetSlotUI = equippedSpellsContainer.GetChild(targetChildIndex).GetComponent<SpellSlotUI>();
        if (targetSlotUI != null)
        {
            targetSlotUI.SetSpell(spell, GetSpellIcon(spell.spellName), this);
            SetSlotIconAlpha(targetSlotUI, 1f);
        }

        if (saveData.equippedSpells == null)
            saveData.equippedSpells = new List<EquippedSpells>();

        saveData.equippedSpells.RemoveAll(e => e.SpellName == spell.spellName || e.SpellSlot == globalSlotNumber);
        saveData.equippedSpells.Add(new EquippedSpells(spell, globalSlotNumber));
        SaveSystem.Save(saveData);
    }

    public void UnequipDisplayedSpell()
    {
        if (selectedSlot == null || selectedSlot.spellData == null) return;

        string spellToRemove = selectedSlot.spellData.spellName;
        if (!IsSpellEquipped(spellToRemove)) return;

        saveData.equippedSpells.RemoveAll(e => e != null && e.SpellName == spellToRemove);
        SaveSystem.Save(saveData);

        if (equippedSpellsContainer != null)
        {
            for (int i = 0; i < equippedSpellsContainer.childCount; i++)
            {
                SpellSlotUI slotUI = equippedSpellsContainer.GetChild(i).GetComponent<SpellSlotUI>();
                if (slotUI != null && slotUI.spellData != null && slotUI.spellData.spellName == spellToRemove)
                {
                    slotUI.SetSpell(null, null, this);
                    SetSlotIconAlpha(slotUI, 0f);
                }
            }
        }

        CancelEquipSelection();
        HideSpellInfo();
    }

    private void SetSlotIconAlpha(SpellSlotUI slotUI, float alpha)
    {
        if (slotUI == null) return;

        Image img = slotUI.GetComponent<Image>();
        if (img == null) img = slotUI.GetComponentInChildren<Image>();

        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }

    private void SetSlotVisualTint(SpellSlotUI slotUI, Color tint)
    {
        if (slotUI == null) return;

        Image img = slotUI.GetComponent<Image>();
        if (img == null) img = slotUI.GetComponentInChildren<Image>();
        if (img != null) img.color = tint;
    }

    private void SetSlotLockState(SpellSlotUI slotUI, bool isLocked)
    {
        if (slotUI == null) return;

        Button btn = slotUI.GetComponent<Button>();
        if (btn == null) btn = slotUI.GetComponentInChildren<Button>();

        if (btn != null)
        {
            btn.interactable = !isLocked;
        }
    }

    private int GetUnlockedSlotCountForElement(SpellsList.Element element)
    {
        switch (element)
        {
            case SpellsList.Element.Fire: return saveData.fireSlots;
            case SpellsList.Element.Water: return saveData.waterSlots;
            case SpellsList.Element.Earth: return saveData.earthSlots;
            case SpellsList.Element.Wind: return saveData.windSlots;
            case SpellsList.Element.Lightning: return saveData.lightningSlots;
            default: return 1;
        }
    }

    // =========================================================
    // 2. SKILL TREE PAGE: Reads existing child prefabs per element
    //    + 5 Unlock Buttons (Assign in Inspector -> Button OnClick)
    // =========================================================
    public void LoadSkillTree()
    {
        PopulateElementBranch(fireTreeParent, SpellsList.Element.Fire);
        PopulateElementBranch(waterTreeParent, SpellsList.Element.Water);
        PopulateElementBranch(earthTreeParent, SpellsList.Element.Earth);
        PopulateElementBranch(windTreeParent, SpellsList.Element.Wind);
        PopulateElementBranch(lightTreeParent, SpellsList.Element.Lightning);
    }

    private void PopulateElementBranch(Transform elementParent, SpellsList.Element element)
    {
        if (elementParent == null) return;

        List<SpellsList> elementSpells = allSpells.FindAll(s => s.element == element);

        int spellIndex = 0;
        for (int i = 0; i < elementParent.childCount; i++)
        {
            SpellSlotUI slot = elementParent.GetChild(i).GetComponent<SpellSlotUI>();
            if (slot == null) continue;

            if (spellIndex < elementSpells.Count)
            {
                SpellsList spell = elementSpells[spellIndex];
                bool unlocked = IsSpellUnlocked(spell.spellName);
                slot.SetSpell(spell, GetSpellIcon(spell.spellName), this);

                SetSlotLockState(slot, false);
                SetSlotVisualTint(slot, unlocked ? Color.white : new Color(0.45f, 0.45f, 0.45f, 1f));
                spellIndex++;
            }
        }
    }

    // --- PAGE 2 BUTTONS (Wire each one to its Skill Tree Unlock Button OnClick) ---
    public void UnlockNextFireSpell() => UnlockNextSpellInTree(fireTreeParent, SpellsList.Element.Fire);
    public void UnlockNextWaterSpell() => UnlockNextSpellInTree(waterTreeParent, SpellsList.Element.Water);
    public void UnlockNextEarthSpell() => UnlockNextSpellInTree(earthTreeParent, SpellsList.Element.Earth);
    public void UnlockNextWindSpell() => UnlockNextSpellInTree(windTreeParent, SpellsList.Element.Wind);
    public void UnlockNextLightningSpell() => UnlockNextSpellInTree(lightTreeParent, SpellsList.Element.Lightning);

    private void UnlockNextSpellInTree(Transform treeParent, SpellsList.Element element)
    {
        if (saveData == null) saveData = SaveSystem.Load() ?? new SaveData();
        if (saveData.unlockedSpellNames == null) saveData.unlockedSpellNames = new List<string>();

        SpellsList nextLockedSpell = null;

        if (treeParent != null)
        {
            for (int i = 0; i < treeParent.childCount; i++)
            {
                SpellSlotUI slot = treeParent.GetChild(i).GetComponent<SpellSlotUI>();
                if (slot != null && slot.spellData != null && !IsSpellUnlocked(slot.spellData.spellName))
                {
                    nextLockedSpell = slot.spellData;
                    break;
                }
            }
        }

        if (nextLockedSpell == null)
        {
            nextLockedSpell = allSpells.FirstOrDefault(s => s.element == element && !IsSpellUnlocked(s.spellName));
        }

        if (nextLockedSpell == null) return;

        int dynamicCost = GetNextSpellUnlockCost(element);
        if (!TrySpendElementCoin(element, dynamicCost)) return;

        saveData.unlockedSpellNames.Add(nextLockedSpell.spellName);
        SaveSystem.Save(saveData);

        LoadSpellList();
        LoadSkillTree();
    }

    private bool TrySpendElementCoin(SpellsList.Element element, int cost)
    {
        if (cost <= 0) return true;

        switch (element)
        {
            case SpellsList.Element.Fire:
                if (saveData.Firecoin < cost) return false;
                saveData.Firecoin -= cost;
                return true;

            case SpellsList.Element.Water:
                if (saveData.Watercoin < cost) return false;
                saveData.Watercoin -= cost;
                return true;

            case SpellsList.Element.Earth:
                if (saveData.Earthcoin < cost) return false;
                saveData.Earthcoin -= cost;
                return true;

            case SpellsList.Element.Wind:
                if (saveData.Windcoin < cost) return false;
                saveData.Windcoin -= cost;
                return true;

            case SpellsList.Element.Lightning:
                if (saveData.Lightcoin < cost) return false;
                saveData.Lightcoin -= cost;
                return true;

            default:
                return false;
        }
    }
    // =========================================================
    // 3. UPGRADES PAGE: Updates Level1..Level9 from unlit to lit
    //    + 8 Upgrade Buttons (Assign in Inspector -> Button OnClick)
    // =========================================================
    public void LoadUpgradesPage()
    {
        UpdateUpgradeBar(atkUpParent, saveData.dmgLevel);
        UpdateUpgradeBar(spdUpParent, saveData.speedLevel);
        UpdateUpgradeBar(hpUpParent, saveData.hpLevel);

        UpdateUpgradeBar(fireSlotsParent, saveData.fireSlots * 3);
        UpdateUpgradeBar(iceSlotsParent, saveData.waterSlots * 3);
        UpdateUpgradeBar(earthSlotsParent, saveData.earthSlots * 3);
        UpdateUpgradeBar(windSlotsParent, saveData.windSlots * 3);
        UpdateUpgradeBar(lightningSlotsParent, saveData.lightningSlots * 3);
    }

    // --- PAGE 3 STAT UPGRADE BUTTONS (Max 9 levels, dynamic gold cost from saveData) ---
    public void UpgradeDamage()
    {
        if (saveData == null) saveData = SaveSystem.Load() ?? new SaveData();
        if (saveData.dmgLevel >= 9) return;
        if (!TrySpendGold(GetNextStatUpgradeCost(saveData.dmgLevel))) return;

        saveData.dmgLevel++;
        SaveSystem.Save(saveData);
        LoadUpgradesPage();
    }

    public void UpgradeSpeed()
    {
        if (saveData == null) saveData = SaveSystem.Load() ?? new SaveData();
        if (saveData.speedLevel >= 9) return;
        if (!TrySpendGold(GetNextStatUpgradeCost(saveData.speedLevel))) return;

        saveData.speedLevel++;
        SaveSystem.Save(saveData);
        LoadUpgradesPage();
    }

    public void UpgradeHp()
    {
        if (saveData == null) saveData = SaveSystem.Load() ?? new SaveData();
        if (saveData.hpLevel >= 9) return;
        if (!TrySpendGold(GetNextStatUpgradeCost(saveData.hpLevel))) return;

        saveData.hpLevel++;
        SaveSystem.Save(saveData);
        LoadUpgradesPage();
    }

    // --- PAGE 3 ELEMENT SLOT UPGRADE BUTTONS (Max 3 slots = 9 pips, dynamic coin cost from saveData) ---
    public void UpgradeFireSlots() => TryUpgradeElementSlot(SpellsList.Element.Fire);
    public void UpgradeIceSlots() => TryUpgradeElementSlot(SpellsList.Element.Water);
    public void UpgradeEarthSlots() => TryUpgradeElementSlot(SpellsList.Element.Earth);
    public void UpgradeWindSlots() => TryUpgradeElementSlot(SpellsList.Element.Wind);
    public void UpgradeLightningSlots() => TryUpgradeElementSlot(SpellsList.Element.Lightning);

    private void TryUpgradeElementSlot(SpellsList.Element element)
    {
        if (saveData == null) saveData = SaveSystem.Load() ?? new SaveData();

        int currentSlots = GetUnlockedSlotCountForElement(element);
        if (currentSlots >= 3) return; // Max 3 equip slots (9 lit pips) per element
        if (!TrySpendElementCoin(element, GetNextSlotUpgradeCost(element))) return;

        switch (element)
        {
            case SpellsList.Element.Fire: saveData.fireSlots++; break;
            case SpellsList.Element.Water: saveData.waterSlots++; break;
            case SpellsList.Element.Earth: saveData.earthSlots++; break;
            case SpellsList.Element.Wind: saveData.windSlots++; break;
            case SpellsList.Element.Lightning: saveData.lightningSlots++; break;
        }

        SaveSystem.Save(saveData);
        LoadUpgradesPage();
        LoadEquippedSlotsFromSave(); // Immediately unlocks the new equip slot on Page 1
    }

    private bool TrySpendGold(int cost)
    {
        if (cost <= 0) return true;
        if (saveData.totalGold < cost) return false;

        saveData.totalGold -= cost;
        return true;
    }

    private void UpdateUpgradeBar(Transform barParent, int litCount)
    {
        if (barParent == null) return;

        for (int i = 0; i < barParent.childCount; i++)
        {
            Transform segment = barParent.GetChild(i);
            bool isLit = i < litCount;
            Sprite targetSprite = isLit ? litSegmentSprite : unlitSegmentSprite;
            if (targetSprite == null) continue;

            Image uiImage = segment.GetComponent<Image>();
            if (uiImage != null)
            {
                uiImage.sprite = targetSprite;
                continue;
            }

            SpriteRenderer spriteRenderer = segment.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = targetSprite;
            }
        }
    }

    // ---------------------------------------------------------
    // INFORMATION DISPLAY (Floating Tooltip + Unequip Button)
    // ---------------------------------------------------------
    private void SelectInitialSpellForDisplay()
    {
        HideSpellInfo();
    }

    public void DisplaySpellInfo(SpellSlotUI pressedSlot)
    {
        if (pressedSlot == null || pressedSlot.spellData == null) return;

        selectedSlot = pressedSlot;
        SpellsList spell = pressedSlot.spellData;
        Sprite iconToShow = pressedSlot.loadedIcon != null ? pressedSlot.loadedIcon : GetSpellIcon(spell.spellName);

        UpdateDisplayUI(spell, iconToShow);

        if (unequipButton != null)
            unequipButton.gameObject.SetActive(IsSpellEquipped(spell.spellName));

        PositionTooltipRelativeToSlot(pressedSlot.GetComponent<RectTransform>());
    }

    private void PositionTooltipRelativeToSlot(RectTransform targetSlotRect)
    {
        if (infoTooltipPanel == null || targetSlotRect == null) return;

        infoTooltipPanel.gameObject.SetActive(true);
        infoTooltipPanel.SetAsLastSibling();

        Canvas parentCanvas = infoTooltipPanel.GetComponentInParent<Canvas>();
        Camera uiCamera = (parentCanvas != null && parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            ? parentCanvas.worldCamera
            : null;

        // 1. Measure the clicked slot's exact top and bottom edges in world & screen space
        Vector3[] slotCorners = new Vector3[4];
        targetSlotRect.GetWorldCorners(slotCorners);

        Vector3 slotBottomWorld = (slotCorners[0] + slotCorners[3]) * 0.5f;
        Vector3 slotTopWorld = (slotCorners[1] + slotCorners[2]) * 0.5f;

        Vector2 slotBottomScreen = RectTransformUtility.WorldToScreenPoint(uiCamera, slotBottomWorld);
        Vector2 slotTopScreen = RectTransformUtility.WorldToScreenPoint(uiCamera, slotTopWorld);

        // 2. Measure the tooltip's TRUE screen height in pixels (immune to the 96x MenuBook scale)
        Vector3[] tipCorners = new Vector3[4];
        infoTooltipPanel.GetWorldCorners(tipCorners);

        Vector2 tipBottomScreen = RectTransformUtility.WorldToScreenPoint(uiCamera, tipCorners[0]);
        Vector2 tipTopScreen = RectTransformUtility.WorldToScreenPoint(uiCamera, tipCorners[1]);
        float tooltipHeightScreen = Mathf.Abs(tipTopScreen.y - tipBottomScreen.y);

        float scaleFactor = (parentCanvas != null) ? parentCanvas.scaleFactor : 1f;
        float offsetScreen = tooltipVerticalOffset * scaleFactor;

        // 3. Place below by default; flip above if bottom would clip off-screen (and room exists above)
        bool fitsBelow = (slotBottomScreen.y - tooltipHeightScreen - offsetScreen) >= 0f;
        bool fitsAbove = (slotTopScreen.y + tooltipHeightScreen + offsetScreen) <= Screen.height;
        bool placeBelow = fitsBelow || !fitsAbove;

        float canvasUnitToWorld = (parentCanvas != null) ? parentCanvas.transform.lossyScale.y : 1f;
        float worldOffset = tooltipVerticalOffset * canvasUnitToWorld;

        if (placeBelow)
        {
            infoTooltipPanel.pivot = new Vector2(0.5f, 1f);
            infoTooltipPanel.position = slotBottomWorld - new Vector3(0f, worldOffset, 0f);
        }
        else
        {
            infoTooltipPanel.pivot = new Vector2(0.5f, 0f);
            infoTooltipPanel.position = slotTopWorld + new Vector3(0f, worldOffset, 0f);
        }
    }

    public void HideSpellInfo()
    {
        if (infoTooltipPanel != null)
            infoTooltipPanel.gameObject.SetActive(false);
    }
    private void UpdateDisplayUI(SpellsList spell, Sprite icon)
    {
        if (spell == null) return;

        if (infoIcon != null)
        {
            infoIcon.sprite = icon;
            infoIcon.enabled = (icon != null);
        }

        if (infoNameText != null)
            infoNameText.text = spell.spellName;

        if (infoDescriptionText != null)
            infoDescriptionText.text = spell.Desc;

        if (infoDamageText != null)
            infoDamageText.text = $"Damage: {spell.baseDamage} (x{spell.scaling})";

        if (infoCooldownText != null)
            infoCooldownText.text = $"Cooldown: {spell.spellCooldown}s";

        if (infoSequenceText != null)
            infoSequenceText.text = (spell.sequence != null && spell.sequence.Count > 0)
                ? string.Join(" - ", spell.sequence)
                : "None";
    }
}