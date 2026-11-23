using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [Range(0f, 1f)] public float masterVolume = 0.5f; //Cool range method
    public float crossfadeDuration = 1f;
    public AudioClip startingTrack;
    private AudioSource activeSource;
    private AudioSource secondarySource;
    private AudioSource buttonVFX;
    private AudioSource swappingSource;
    private Coroutine fadeRoutine;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        activeSource = gameObject.AddComponent<AudioSource>();
        secondarySource = gameObject.AddComponent<AudioSource>();
        swappingSource = gameObject.AddComponent<AudioSource>();
        buttonVFX = gameObject.AddComponent<AudioSource>();
        activeSource.loop = true;
        secondarySource.loop = true;
        activeSource.playOnAwake = false;
        secondarySource.playOnAwake = false;
        buttonVFX.loop = false;
        buttonVFX.playOnAwake =false;
    }
    void Start()
    {
        if (startingTrack != null)
            PlayMusic(startingTrack, false);
    }
    public void PlayMusic(AudioClip newClip, bool crossfade = true)
    {
        if (activeSource.clip == newClip && activeSource.isPlaying) return;
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        if (!crossfade || !activeSource.isPlaying)
        {
            activeSource.clip = newClip;
            activeSource.volume = masterVolume;
            //activeSource.Play();
        }
        else
        {
            fadeRoutine = StartCoroutine(CrossfadeRoutine(newClip));
        }
    }
    public void ButtonHover()
    {
        buttonVFX.clip = Resources.Load<AudioClip>("Audio Files/MenuVFX/ButtonHover");
        buttonVFX.volume = masterVolume;
        buttonVFX.Play();
    }
    public void ButtonPress()
    {
        buttonVFX.clip = Resources.Load<AudioClip>("Audio Files/MenuVFX/ButtonP1");
        buttonVFX.volume = masterVolume;
        buttonVFX.Play();
    }
    public void SetVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        activeSource.volume = masterVolume;
    }
    public void StopMusic()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        activeSource.Stop();
        secondarySource.Stop();
    }
    private IEnumerator CrossfadeRoutine(AudioClip newClip)
    {
        secondarySource.clip = newClip;
        secondarySource.volume = 0f;
        secondarySource.Play();
        float timer = 0f;
        float startVolume = activeSource.volume;
        while (timer < crossfadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / crossfadeDuration;
            activeSource.volume = Mathf.Lerp(startVolume, 0f, t);
            secondarySource.volume = Mathf.Lerp(0f, masterVolume, t);
            yield return null;
        }
        activeSource.Stop();
        activeSource.volume = 0f;
        secondarySource.volume = masterVolume;
        (activeSource, secondarySource) = (secondarySource, activeSource);
    }
}