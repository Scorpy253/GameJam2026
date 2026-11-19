using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlotMenu : MonoBehaviour
{
    public const int SlotCount = 16;

    [SerializeField] private PlayerInformation playerInformation;
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private GameObject slotPrefab;

    private readonly EquippedSpells[] slotSpells = new EquippedSpells[SlotCount];
    private readonly List<GameObject> spawnedSlots = new List<GameObject>();

    private void Start()
    {
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            slotSpells[i] = null;
        }

        if (playerInformation != null && playerInformation.currentSpells != null)
        {
            for (int i = 0; i < playerInformation.currentSpells.Count; i++)
            {
                EquippedSpells entry = playerInformation.currentSpells[i];
                if (entry == null) continue;

                // Use 1-based SpellSlot if in range 1..16, otherwise fall back to list index
                int index = (entry.SpellSlot >= 1 && entry.SpellSlot <= SlotCount)
                    ? entry.SpellSlot - 1
                    : i;

                if (index >= 0 && index < SlotCount)
                {
                    slotSpells[index] = entry;
                }
            }
        }

        DrawSlots();
    }

    private void DrawSlots()
    {
        foreach (GameObject slot in spawnedSlots)
        {
            Destroy(slot);
        }
        spawnedSlots.Clear();

        if (slotsContainer == null || slotPrefab == null) return;

        for (int i = 0; i < SlotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
            spawnedSlots.Add(slotObj);

            EquippedSpells spell = slotSpells[i];
            Image iconImg = slotObj.transform.Find("Icon")?.GetComponent<Image>() ?? slotObj.GetComponent<Image>();
            if (iconImg != null)
            {
                iconImg.enabled = spell != null;
            }
        }
    }
}