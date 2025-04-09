using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private Transform soundsParent;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public void playSoundEffect(AudioClip audioClip, Transform transform, float volume)
    {
        // Create audio game object.
        GameObject audioObject = new GameObject();
        audioObject.transform.parent = getSoundsParent();
        audioObject.transform.position = transform.position;
        audioObject.transform.rotation = Quaternion.identity;

        // Create audio source.
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.transform.parent = getSoundsParent();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        // Destroy after audio plays.
        float clipLength = audioSource.clip.length;
        Destroy(audioObject, clipLength);
    }

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
