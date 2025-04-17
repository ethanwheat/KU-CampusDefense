using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Settings Data Controller")]
    [SerializeField] private SettingsDataController settingsDataController;

    private Transform soundsParent;
    private GameObject musicObject;

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
    }

    void Start()
    {
        // Play music if playMusicOnStart is true.
        if (playMusicOnStart)
        {
            playMusic(music, transform, .5f, .5f);
        }
    }

    // Play music.
    public void playMusic(AudioClip audioClip, Transform transform, float volume, float duration)
    {
        StartCoroutine(playMusicCoroutine(audioClip, transform, volume, duration));
    }

    // Stop music.
    public void stopMusic(float duration)
    {
        StartCoroutine(stopMusicCoroutine(duration));
    }

    // Set music volume.
    public void onMusicVolumeChange(float volume)
    {
        settingsDataController.setMusicVolume(volume);

        AudioSource audioSource = musicObject.GetComponent<AudioSource>();
        audioSource.volume = volume / 2;
    }

    // Play music coroutine.
    IEnumerator playMusicCoroutine(AudioClip audioClip, Transform transform, float volume, float duration)
    {
        yield return stopMusicCoroutine(duration);

        // Get music volume.
        float musicVolume = settingsDataController.getMusicVolume();

        // Create audio game object.
        musicObject = createSoundObject(audioClip, transform);
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
            // Interpolate between the start and end colors
            audioSource.volume = Mathf.Lerp(0, volume * musicVolume, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    // Stop music coroutine.
    IEnumerator stopMusicCoroutine(float duration)
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
                // Interpolate between the start and end colors
                audioSource.volume = Mathf.Lerp(volume, 0, timeElapsed / duration);
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            // Destroy music object.
            Destroy(musicObject);
        }
    }

    // Play sound effect.
    public void playSoundEffect(AudioClip audioClip, Transform transform, float volume)
    {
        // Get sound effect volume.
        float soundEffectsVolume = settingsDataController.getSoundEffectsVolume();

        // Create audio game object.
        GameObject soundEffectObject = createSoundObject(audioClip, transform);
        soundEffectObject.transform.parent = soundEffectsParent;

        // Create audio source.
        AudioSource audioSource = soundEffectObject.GetComponent<AudioSource>();
        audioSource.volume = volume * soundEffectsVolume;
        audioSource.Play();

        // Destroy after audio plays.
        float clipLength = audioSource.clip.length + .1f;
        Destroy(soundEffectObject, clipLength);
    }

    // Create music object.
    GameObject createSoundObject(AudioClip audioClip, Transform transform)
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
