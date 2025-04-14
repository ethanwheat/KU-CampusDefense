using UnityEngine;

[CreateAssetMenu(fileName = "SetttingsDataController", menuName = "Scriptable Objects/SetttingsDataController")]
public class SettingsDataController : ScriptableObject
{
    [Header("Volume")]
    [SerializeField] private float musicVolume = 1;
    [SerializeField] private float soundEffectsVolume = 1;

    // Get music volume.
    public float getMusicVolume()
    {
        return musicVolume;
    }

    // Set music volume.
    public void setMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    // Get sound effects volume.
    public float getSoundEffectsVolume()
    {
        return soundEffectsVolume;
    }

    // Set sound effects volume.
    public void setSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = volume;
    }

    // Reset settings.
    public void resetSettings()
    {
        musicVolume = 1;
        soundEffectsVolume = 1;
    }
}
