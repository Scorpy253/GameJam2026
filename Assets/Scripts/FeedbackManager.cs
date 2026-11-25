using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class FeedbackManager : MonoBehaviour
{
    private static FeedbackManager runner;
    [Header("Optional Scene Overrides")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Volume globalVolume;
    private Vector3 initialCamLocalPos;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private Coroutine shakeRoutine;
    private Coroutine filterRoutine;
    private Coroutine hitstopRoutine;
    private bool isInitialized;
    private static FeedbackManager Runner
    {
        get
        {
            if (runner == null)
            {
                runner = FindAnyObjectByType<FeedbackManager>();
                if (runner == null)
                {
                    var go = new GameObject("[FeedbackManager]");
                    runner = go.AddComponent<FeedbackManager>();
                }
                runner.Initialize();
            }
            return runner;
        }
    }
    private void Awake()
    {
        if (runner != null && runner != this)
        {
            Destroy(gameObject);
            return;
        }
        runner = this;
        Initialize();
    }
    private void Initialize()
    {
        if (isInitialized) return;
        isInitialized = true;
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
        initialCamLocalPos = cameraTransform.localPosition;
        if (globalVolume == null)
            globalVolume = FindAnyObjectByType<Volume>();
        globalVolume.profile.TryGet(out vignette);
        globalVolume.profile.TryGet(out chromaticAberration);
    }
    public static void Shake(float force, float duration)
    {
        var r = Runner;
        if (r.shakeRoutine != null) r.StopCoroutine(r.shakeRoutine);
        r.shakeRoutine = r.StartCoroutine(r.ShakeCoroutine(force, duration));
    }
    public static void PulseFilter(Color color, float vignetteIntensity, float chromaticIntensity, float duration)
    {
        var r = Runner;
        if (r.filterRoutine != null) r.StopCoroutine(r.filterRoutine);
        r.filterRoutine = r.StartCoroutine(r.FilterCoroutine(color, vignetteIntensity, chromaticIntensity, duration));
    }
    public static void Hitstop(float duration, float timeScale = 0.05f)
    {
        var r = Runner;
        if (r.hitstopRoutine != null) r.StopCoroutine(r.hitstopRoutine);
        r.hitstopRoutine = r.StartCoroutine(r.HitstopCoroutine(duration, timeScale));
    }
    public static void Impact(float shakeForce, float shakeDuration, Color flashColor, float vignetteIntensity = 0.35f, float chromaticIntensity = 0.4f, float filterDuration = 0.25f, float hitstopDuration = 0f)
    {
        if (shakeForce > 0f && shakeDuration > 0f)
            Shake(shakeForce, shakeDuration);
        if (filterDuration > 0f && (vignetteIntensity > 0f || chromaticIntensity > 0f))
            PulseFilter(flashColor, vignetteIntensity, chromaticIntensity, filterDuration);
        if (hitstopDuration > 0f)
            Hitstop(hitstopDuration);
    }
    private IEnumerator ShakeCoroutine(float force, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float damper = 1f - (elapsed / duration);
            Vector2 offset = Random.insideUnitCircle * (force * damper);
            cameraTransform.localPosition = initialCamLocalPos + new Vector3(offset.x, offset.y, 0f);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        cameraTransform.localPosition = initialCamLocalPos;
        shakeRoutine = null;
    }
    private IEnumerator FilterCoroutine(Color flashColor, float targetVignette, float targetChromatic, float duration)
    {
        vignette.active = true;
        vignette.color.overrideState = true;
        vignette.intensity.overrideState = true;
        vignette.color.value = flashColor;
        chromaticAberration.active = true;
        chromaticAberration.intensity.overrideState = true;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = 1f - (elapsed / duration);
            vignette.intensity.value = targetVignette * t;
            chromaticAberration.intensity.value = targetChromatic * t;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        vignette.intensity.value = 0f;
        chromaticAberration.intensity.value = 0f;
        filterRoutine = null;
    }
    private IEnumerator HitstopCoroutine(float duration, float timeScale)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        hitstopRoutine = null;
    }
}