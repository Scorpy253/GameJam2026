using UnityEngine;
using UnityEngine.UI;

public class SpellSlotUI : MonoBehaviour
{
    [HideInInspector] public SpellsList spellData;
    [HideInInspector] public Sprite loadedIcon;
    [HideInInspector] public MenuShop menuShop;

    private Image slotImage;

    public void SetSpell(SpellsList spell, Sprite icon, MenuShop shop)
    {
        spellData = spell;
        loadedIcon = icon;
        menuShop = shop;

        if (slotImage == null) slotImage = GetComponent<Image>();

        if (slotImage != null)
        {
            if (icon != null) slotImage.sprite = icon;
            slotImage.color = Color.white;
        }
    }
    // Wire your Prefab's Button OnClick() to this method in the Inspector
    public void OnSlotPressed()
    {
        if (menuShop != null && spellData != null)
            menuShop.DisplaySpellInfo(this);
    }
}