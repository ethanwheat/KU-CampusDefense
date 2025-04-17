using UnityEngine;

[CreateAssetMenu(fileName = "SetttingsDataController", menuName = "Scriptable Objects/SetttingsDataController")]
public class SettingsDataController : ScriptableObject
{
    [Header("Volume")]
    [SerializeField] private float musicVolume = 1;
    [SerializeField] private float soundEffectsVolume = 1;

    public float MusicVolume => musicVolume;
    public float SoundEffectsVolume => soundEffectsVolume;

    // Set music volume.
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    // Set sound effects volume.
    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = volume;
    }

    // Reset settings.
    public void ResetSettings()
    {
        musicVolume = 1;
        soundEffectsVolume = 1;
    }
}
