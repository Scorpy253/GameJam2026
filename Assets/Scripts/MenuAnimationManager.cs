using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Required for Mouse.current
using System.Collections;

public class MenuAnimationManager : MonoBehaviour
{
    public static MenuAnimationManager instance;

    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    [Header("Parallax Settings")]
    [Tooltip("Drag your menu art layer here (Background, Midground, etc.)")]
    public RectTransform[] parallaxLayers;
    [Tooltip("Match the intensity values to your layer (higher = more movement)")]
    public float[] parallaxIntensities;
    public float smoothness = 5f;
    private bool isTransitioning = false;
    private Vector2[] initialPositions;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            CacheParallaxPositions();
        }
        else
        {
            // Hand the newly loaded MainMenu's parallax layers to the surviving fader
            instance.parallaxLayers = this.parallaxLayers;
            instance.parallaxIntensities = this.parallaxIntensities;
            instance.smoothness = this.smoothness;
            instance.CacheParallaxPositions();

            Destroy(gameObject);
            return;
        }
    }
    void Update()
    {
        if (!isTransitioning)
        {
            ApplyParallax();
        }
    }
    public void CacheParallaxPositions()
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
        if (parallaxLayers == null || initialPositions == null || Mouse.current == null)
            return;

        // Read mouse position using the New Input System
        Vector2 rawMouse = Mouse.current.position.ReadValue();

        // Normalize mouse position (-0.5 to 0.5)
        Vector2 mousePos = new Vector2(
            (rawMouse.x / Screen.width) - 0.5f,
            (rawMouse.y / Screen.height) - 0.5f
        );

        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            if (parallaxLayers[i] != null)
            {
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

    public void FadeToScene(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(FadeRoutine(sceneName));
    }

    IEnumerator FadeRoutine(string sceneName)
    {
        isTransitioning = true;
        canvasGroup.blocksRaycasts = true;
        float timer = 0f;

        // 1. FADE IN (Alpha 0 -> 1)
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // 2. Clear parallax layers before loading next scene
        parallaxLayers = null;

        // 3. LOAD SCENE
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }

        // 4. FADE OUT (Alpha 1 -> 0)
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = 1f - (timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        parallaxLayers = null;
        initialPositions = null;
        canvasGroup.blocksRaycasts = false;
        isTransitioning = false;
    }
}