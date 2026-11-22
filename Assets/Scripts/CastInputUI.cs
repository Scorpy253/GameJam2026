using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CastInputUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform spellListContainer;
    [SerializeField] private GameObject spellRowPrefab;
    [SerializeField] private Image arrowIconPrefab;
    [Header("Normal Arrow Sprites")]
    [SerializeField] private Sprite upNormal;
    [SerializeField] private Sprite downNormal;
    [SerializeField] private Sprite leftNormal;
    [SerializeField] private Sprite rightNormal;
    [Header("Glowing Arrow Sprites")]
    [SerializeField] private Sprite upGlow;
    [SerializeField] private Sprite downGlow;
    [SerializeField] private Sprite leftGlow;
    [SerializeField] private Sprite rightGlow;
    [Header("Settings")]
    [Range(0.05f, 1f)]
    [SerializeField] private float translucentAlpha = 0.5f;
    private readonly List<GameObject> spawnedRows = new List<GameObject>();
    private void Awake()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    public void SetCastingState(bool isCasting)
    {
        if (canvasGroup != null)
            canvasGroup.alpha = isCasting ? translucentAlpha : 0f;
        ClearDisplay();
    }
    public void ClearDisplay()
    {
        for (int i = 0; i < spawnedRows.Count; i++)
        {
            if (spawnedRows[i] != null)
            {
                spawnedRows[i].transform.SetParent(null);
                Destroy(spawnedRows[i]);
            }
        }
        spawnedRows.Clear();
    }
    public void AddValidSpell(EquippedSpells spell, int matchedCount)
    {
        if (spellListContainer == null || spellRowPrefab == null || arrowIconPrefab == null || spell == null)
            return;
        GameObject rowObj = Instantiate(spellRowPrefab, spellListContainer);
        Image iconImg = rowObj.GetComponent<Image>();
        if (iconImg != null)
        {
            Sprite loadedSprite = Resources.Load<Sprite>($"SpellIcons/{spell.SpellName}");
            if (loadedSprite == null)
                Debug.LogWarning($"Icon not found at Resources/SpellIcons/{spell.SpellName}");
            iconImg.sprite = loadedSprite;
        }
        Transform arrowHolder = rowObj.transform.GetChild(0);
        for (int i = 0; i < spell.SpellArrow.Length; i++)
        {
            Image arrowImg = Instantiate(arrowIconPrefab, arrowHolder);
            bool isGlowing = i < matchedCount;
            arrowImg.sprite = GetArrowSprite(spell.SpellArrow[i], isGlowing);
        }
        spawnedRows.Add(rowObj);
    }
    private Sprite GetArrowSprite(SpellsList.Arrow arrow, bool glowing) => (arrow, glowing) switch
    {
        (SpellsList.Arrow.Up, false) => upNormal,
        (SpellsList.Arrow.Down, false) => downNormal,
        (SpellsList.Arrow.Left, false) => leftNormal,
        (SpellsList.Arrow.Right, false) => rightNormal,
        (SpellsList.Arrow.Up, true) => upGlow,
        (SpellsList.Arrow.Down, true) => downGlow,
        (SpellsList.Arrow.Left, true) => leftGlow,
        (SpellsList.Arrow.Right, true) => rightGlow,
        _ => upNormal
    };
}