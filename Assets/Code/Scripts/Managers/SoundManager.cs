using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Sound Manager Settings")]
    [SerializeField] private bool playMusicOnStart;

    [Header("Music")]
    [SerializeField] private AudioClip music;

    [Header("Parents")]
    [SerializeField] private Transform musicParent;
    [SerializeField] private Transform soundEffectsParent;

    private float musicVolume;
    private float soundEffectsVolume;
    private GameObject musicObject;
    private Coroutine musicCoroutine;

    public float MusicVolume => musicVolume;
    public float SoundEffectsVolume => soundEffectsVolume;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1f);
        }

        if (!PlayerPrefs.HasKey("SoundEffectsVolume"))
        {
            PlayerPrefs.SetFloat("SoundEffectsVolume", 1f);
        }

        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        soundEffectsVolume = PlayerPrefs.GetFloat("SoundEffectsVolume");
    }

    void Start()
    {
        // Play music if playMusicOnStart is true.
        if (playMusicOnStart)
        {
            PlayMusic(music, transform);
        }
    }

    // Play music.
    public void PlayMusic(AudioClip audioClip, Transform transform, float volume = .5f, float duration = .5f)
    {
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
        }

        musicCoroutine = StartCoroutine(PlayMusicCoroutine(audioClip, transform, volume, duration));
    }

    // Resume Music
    public void ResumeMusic()
    {
        PlayMusic(music, transform);
    }

    // Stop music.
    public void StopMusic(float duration = .5f)
    {
        StartCoroutine(StopMusicCoroutine(duration));
    }

    // Set music volume.
    public void OnMusicVolumeChange(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        AudioSource audioSource = musicObject.GetComponent<AudioSource>();
        audioSource.volume = volume / 2;
    }

    // Play music coroutine.
    IEnumerator PlayMusicCoroutine(AudioClip audioClip, Transform transform, float volume, float duration)
    {
        yield return StopMusicCoroutine(duration);

        // Create audio game object.
        musicObject = CreateSoundObject(audioClip, transform);
        musicObject.transform.parent = musicParent;

        // Create audio source.
        AudioSource audioSource = musicObject.GetComponent<AudioSource>();
        audioSource.volume = 0;
        audioSource.loop = true;
        audioSource.Play();

        // Fade in music.
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            audioSource.volume = Mathf.Lerp(0f, volume * musicVolume, timeElapsed / duration);
            timeElapsed += Time.timeScale != 0f ? Time.deltaTime : Time.unscaledDeltaTime;

            yield return null;
        }
    }

    // Stop music coroutine.
    IEnumerator StopMusicCoroutine(float duration)
    {
        if (musicObject)
        {
            // Get audio source.
            AudioSource audioSource = musicObject.GetComponent<AudioSource>();

            // Fade out music.
            float volume = audioSource.volume;
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                audioSource.volume = Mathf.Lerp(volume, 0f, timeElapsed / duration);
                timeElapsed += Time.timeScale != 0f ? Time.deltaTime : Time.unscaledDeltaTime;

                yield return null;
            }

            // Destroy music object.
            Destroy(musicObject);
        }
    }

    // Play sound effect.
    public void PlaySoundEffect(AudioClip audioClip, Transform transform, float volume = .5f)
    {
        // Create audio game object.
        GameObject soundEffectObject = CreateSoundObject(audioClip, transform);
        soundEffectObject.transform.parent = soundEffectsParent;

        // Create audio source.
        AudioSource audioSource = soundEffectObject.GetComponent<AudioSource>();
        audioSource.volume = volume * soundEffectsVolume;
        audioSource.Play();

        // Destroy after audio plays.
        float clipLength = audioSource.clip.length + .1f;
        Destroy(soundEffectObject, clipLength);
    }

    // Set music volume.
    public void OnSoundEffectVolumeChange(float volume)
    {
        soundEffectsVolume = volume;
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
    }

    // Create music object.
    GameObject CreateSoundObject(AudioClip audioClip, Transform transform)
    {
        // Create audio game object.
        GameObject audioObject = new GameObject(audioClip.name);
        audioObject.transform.position = transform.position;
        audioObject.transform.rotation = Quaternion.identity;

        // Create audio source.
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;

        return audioObject;
    }
}
