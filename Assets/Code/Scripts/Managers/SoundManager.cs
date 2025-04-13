using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Settings Data Controller")]
    [SerializeField] private SettingsDataController settingsDataController;

    private Transform soundsParent;
    private GameObject musicObject;
    private List<AudioSource> soundEffects = new List<AudioSource>();

    void Awake()
    {
        if (!instance)
        {
            instance = this;
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

        float musicVolume = settingsDataController.getMusicVolume();
        AudioSource audioSource = musicObject.GetComponent<AudioSource>();
        audioSource.volume = musicVolume;
    }

    // Play music coroutine.
    IEnumerator playMusicCoroutine(AudioClip audioClip, Transform transform, float volume, float duration)
    {
        yield return stopMusicCoroutine(duration);

        // Get music volume.
        float musicVolume = settingsDataController.getMusicVolume();

        // Create audio game object.
        musicObject = createSoundObject(audioClip, transform);
        AudioSource audioSource = musicObject.GetComponent<AudioSource>();
        audioSource.volume = 0;
        audioSource.loop = true;
        audioSource.Play();

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
            AudioSource audioSource = musicObject.GetComponent<AudioSource>();
            float volume = audioSource.volume;
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                // Interpolate between the start and end colors
                audioSource.volume = Mathf.Lerp(volume, 0, timeElapsed / duration);
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            Destroy(musicObject);
        }
    }

    // Play sound effect.
    public void playSoundEffect(AudioClip audioClip, Transform transform, float volume)
    {
        // Get sound effect volume.
        float soundEffectsVolume = settingsDataController.getSoundEffectsVolume();

        // Create audio game object.
        GameObject audioObject = createSoundObject(audioClip, transform);
        AudioSource audioSource = audioObject.GetComponent<AudioSource>();
        audioSource.volume = volume * soundEffectsVolume;
        audioSource.Play();

        // Destroy after audio plays.
        float clipLength = audioSource.clip.length + .1f;
        Destroy(audioObject, clipLength);
    }

    // Create music object.
    GameObject createSoundObject(AudioClip audioClip, Transform transform)
    {
        // Create audio game object.
        GameObject audioObject = new GameObject();
        audioObject.transform.parent = getSoundsParent();
        audioObject.transform.position = transform.position;
        audioObject.transform.rotation = Quaternion.identity;
        audioObject.name = audioClip.name;

        // Create audio source.
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.transform.parent = getSoundsParent();
        audioSource.clip = audioClip;

        return audioObject;
    }

    // Get sounds parent.
    Transform getSoundsParent()
    {
        if (soundsParent)
        {
            return soundsParent;
        }

        foreach (GameObject currentGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (currentGameObject.name == "Sounds")
            {
                soundsParent = currentGameObject.transform;
                break;
            }
        }

        if (!soundsParent)
        {
            soundsParent = new GameObject("Sounds").transform;
        }

        return soundsParent;
    }
}
