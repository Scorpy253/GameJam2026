using UnityEngine;
using UnityEngine.UI;
public class SpellSlotUI : MonoBehaviour
{
    [HideInInspector] public SpellsList spellData;
    [HideInInspector] public Sprite loadedIcon;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button slotButton;
    private MenuShop parentMenu;
    public void SetSpell(SpellsList spell, Sprite icon, MenuShop menu)
    {
        spellData = spell;
        loadedIcon = icon;
        parentMenu = menu;
        if (iconImage == null) iconImage = GetComponent<Image>() ?? GetComponentInChildren<Image>();
        if (slotButton == null) slotButton = GetComponent<Button>() ?? GetComponentInChildren<Button>();
        iconImage.sprite = icon;
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(OnSlotClicked);
    }
    private void OnSlotClicked()
    {
        parentMenu.OnSlotPressed(this);
    }
}