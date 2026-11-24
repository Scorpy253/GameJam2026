using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuShop : MonoBehaviour
{
    [Header("Icon Loading Settings")]
    [SerializeField] private string iconFolderPath = "SpellIcons"; // Loads Resources/<iconFolderPath>/<spellName>

    [Header("1. Spell List Page (Dynamic Spawn)")]
    [SerializeField] private Transform spellListContainer;         // Has Horizontal/Grid Layout Group
    [SerializeField] private SpellSlotUI spellSlotPrefab;          // Your pre-wired prefab

    [Header("2. Skill Tree Page (Element Parent Folders)")]
    [SerializeField] private Transform fireTreeParent;               // Children ordered root (0) -> top (N)
    [SerializeField] private Transform waterTreeParent;
    [SerializeField] private Transform earthTreeParent;
    [SerializeField] private Transform windTreeParent;
    [SerializeField] private Transform lightTreeParent;

    [Header("3. Stats Page (Stat Level Sprite Containers)")]
    [SerializeField] private Transform dmgLevelParent;               // Children are level pip Images (0 -> max)
    [SerializeField] private Transform hpLevelParent;
    [SerializeField] private Transform speedLevelParent;
    [SerializeField] private Transform cdrLevelParent;
    [SerializeField] private Sprite unlockedLevelSprite;
    [SerializeField] private Sprite lockedLevelSprite;

    [Header("Spell Information Display")]
    [SerializeField] private Image infoIcon;
    [SerializeField] private TextMeshProUGUI infoNameText;
    [SerializeField] private TextMeshProUGUI infoDamageText;
    [SerializeField] private TextMeshProUGUI infoCooldownText;
    [SerializeField] private TextMeshProUGUI infoSequenceText;

    private SaveData saveData;
    private List<SpellsList> allSpells = new List<SpellsList>();

    private void Awake()
    {
        LoadAllSpellClasses();
    }

    // Automatically finds and instantiates EVERY class that inherits from SpellsList
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
        RefreshAll();
    }
    public void RefreshAll()
    {
        if (allSpells == null || allSpells.Count == 0)
            LoadAllSpellClasses();

        saveData = SaveSystem.Load();
        if (saveData == null) saveData = new SaveData();

        LoadSpellList();
        LoadSkillTree();
        LoadStatsMenu();
    }
    // Loads sprite matching spellName exactly (checks manual list first, then Resources folder)
    private Sprite GetSpellIcon(string spellName)
    {
        string path = string.IsNullOrEmpty(iconFolderPath) ? spellName : $"{iconFolderPath}/{spellName}";
        return Resources.Load<Sprite>(path);
    }
    // ---------------------------------------------------------
    // 1. SPELL LIST: Spawns EVERY spell side-by-side (unrelated to savefile)
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
    }

    // ---------------------------------------------------------
    // 2. SKILL TREE: Reads existing child prefabs per element (root -> top)
    // ---------------------------------------------------------
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
                slot.SetSpell(spell, GetSpellIcon(spell.spellName), this);
                spellIndex++;
            }
        }
    }

    // ---------------------------------------------------------
    // 3. STATS MENU: Updates child sprites up to saved level
    // ---------------------------------------------------------
    public void LoadStatsMenu()
    {
        UpdateStatBar(dmgLevelParent, saveData.dmgLevel);
        UpdateStatBar(hpLevelParent, saveData.hpLevel);
        UpdateStatBar(speedLevelParent, saveData.speedLevel);
        UpdateStatBar(cdrLevelParent, saveData.cdrLevel);
    }

    private void UpdateStatBar(Transform statParent, int unlockedLevel)
    {
        if (statParent == null) return;

        for (int i = 0; i < statParent.childCount; i++)
        {
            Image pipImage = statParent.GetChild(i).GetComponent<Image>();
            if (pipImage == null) continue;

            bool isLevelUnlocked = i < unlockedLevel;
            if (isLevelUnlocked && unlockedLevelSprite != null)
                pipImage.sprite = unlockedLevelSprite;
            else if (!isLevelUnlocked && lockedLevelSprite != null)
                pipImage.sprite = lockedLevelSprite;
        }
    }

    // ---------------------------------------------------------
    // INFORMATION DISPLAY (Called by any SpellSlotUI prefab)
    // ---------------------------------------------------------
    public void DisplaySpellInfo(SpellSlotUI pressedSlot)
    {
        if (pressedSlot == null || pressedSlot.spellData == null) return;

        SpellsList spell = pressedSlot.spellData;

        if (infoIcon != null && pressedSlot.loadedIcon != null) infoIcon.sprite = pressedSlot.loadedIcon;
        if (infoNameText != null) infoNameText.text = spell.spellName;
        if (infoDamageText != null) infoDamageText.text = $"Damage: {spell.baseDamage} (x{spell.scaling})";
        if (infoCooldownText != null) infoCooldownText.text = $"Cooldown: {spell.spellCooldown}s";
        if (infoSequenceText != null) infoSequenceText.text = string.Join(" - ", spell.sequence);
    }
}