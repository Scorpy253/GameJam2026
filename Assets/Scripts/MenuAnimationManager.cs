using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuAnimationManager : MonoBehaviour
{
    public static MenuAnimationManager instance;

    [Header("UI Refrences")]
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    [Header("Paralax Settings")]
    [Tooltip("Drag your menu art layer here (Background, Midground, etc.)")]
    public RectTransform[] parallaxLayers;
    [Tooltip("Match the intensity values to your layer (higher = more movement)")]
    public float[] parallaxIntensities;
    public float smoothness = 5f;

    private bool isTransitioning = false;
    private Vector2[] initialPositions;

    void Awake()
    {
        // Singleton pattern: Ensure only one fader exists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this GameObject alive when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates if returning to the main menu
            return;
        }
    }

    void Start()
    {
        CacheParallaxPositions();
    }

    void Update()
    {
        // Only run parallax if we are in the menu and not currently fading to gameplay
        if (!isTransitioning)
        {
            ApplyParallax();
        }
    }

    void CacheParallaxPositions()
    {
        if (parallaxLayers != null && parallaxLayers.Length > 0)
        {
            initialPositions = new Vector2[parallaxLayers.Length];
            for (int i = 0; i < parallaxLayers.Length; i++)
            {
                if (parallaxLayers[i] != null)
                {
                    initialPositions[i] = parallaxLayers[i].anchoredPosition;
                }
            }
        }
    }

    void ApplyParallax()
    {
        if (parallaxLayers == null || initialPositions == null) 
            return;

        // Get normalized mouse position (-0.5 to 0.5)
        Vector2 mousePos = new Vector2(
            (Input.mousePosition.x / Screen.width) - 0.5f,
            (Input.mousePosition.y / Screen.height) - 0.5f
        );

        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            if (parallaxLayers[i] != null)
            {
                // Fallback intensity if array length doesn't match layers
                float intensity = (i < parallaxIntensities.Length) ? parallaxIntensities[i] : 15f;

                Vector2 targetPosition = initialPositions[i] + (mousePos * intensity);

                parallaxLayers[i].anchoredPosition = Vector2.Lerp(
                    parallaxLayers[i].anchoredPosition,
                    targetPosition,
                    Time.deltaTime * smoothness
                );
            }
        }
    }

    // Public method called by your UI buttons
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeRoutine(sceneName));
    }
    IEnumerator FadeRoutine(string sceneName)
    {
        // Enable blockRaycasts so the player can't click things while fading
        canvasGroup.blocksRaycasts = true;
        float timer = 0f;

        // 1. FADE IN (Alpha 0 -> 1: Screen turns black)
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // 2. Clear parallax layers array so Unity doesn't look for destroyed objects in the next scene
        parallaxLayers = null;

        // 3. LOAD SCENE ASYNCHRONOUSLY
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }

        // 4. FADE OUT (Alpha 1 -> 0: Screen reveals the new scene)
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = 1f - (timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;

        // Disable blockRaycasts so gameplay inputs work again
        canvasGroup.blocksRaycasts = false;
        isTransitioning = false;
    }
}
