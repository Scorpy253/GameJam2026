using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastInputUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform arrowContainer;
    [SerializeField] private Image arrowIconPrefab;

    [Header("Arrow Sprites")]
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    [Header("Translucent Setting")]
    [Range(0.05f, 1f)]
    [SerializeField] private float translucentAlpha = 0.5f;
    [SerializeField] private float popScale = 1.2f;

    private readonly List<Image> activeIcons = new List<Image>();

    private void Awake()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void SetCastingState(bool isCasting, List<SpellsList.Arrow> currentInputs)
    {
        if (canvasGroup != null)
            canvasGroup.alpha = isCasting ? translucentAlpha : 0f;

        if (!isCasting)
        {
            ClearIcons();
            return;
        }

        UpdateInputs(currentInputs);
    }

    public void UpdateInputs(List<SpellsList.Arrow> inputs)
    {
        if (arrowContainer == null || arrowIconPrefab == null) return;

        int count = inputs != null ? inputs.Count : 0;

        while (activeIcons.Count > count)
        {
            int last = activeIcons.Count - 1;
            if (activeIcons[last] != null)
                Destroy(activeIcons[last].gameObject);
            activeIcons.RemoveAt(last);
        }

        for (int i = 0; i < count; i++)
        {
            bool isNew = i >= activeIcons.Count;
            Image icon = isNew ? Instantiate(arrowIconPrefab, arrowContainer) : activeIcons[i];

            icon.sprite = GetSprite(inputs[i]);
            icon.rectTransform.localScale = isNew ? Vector3.one * popScale : Vector3.one;

            if (isNew)
                activeIcons.Add(icon);
        }
    }

    private void ClearIcons()
    {
        for (int i = 0; i < activeIcons.Count; i++)
        {
            if (activeIcons[i] != null)
                Destroy(activeIcons[i].gameObject);
        }
        activeIcons.Clear();
    }

    private Sprite GetSprite(SpellsList.Arrow arrow) => arrow switch
    {
        SpellsList.Arrow.Up => upSprite,
        SpellsList.Arrow.Down => downSprite,
        SpellsList.Arrow.Left => leftSprite,
        SpellsList.Arrow.Right => rightSprite,
        _ => upSprite
    };
}