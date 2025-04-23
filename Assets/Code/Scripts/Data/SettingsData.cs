[System.Serializable]

public class SettingsData
{
    public float MusicVolume = 1;
    public float SoundEffectsVolume = 1;

    // Set music volume.
    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
    }

    // Set sound effects volume.
    public void SetSoundEffectsVolume(float volume)
    {
        SoundEffectsVolume = volume;
    }

    // Reset settings.
    public void ResetSettings()
    {
        MusicVolume = 1;
        SoundEffectsVolume = 1;
    }
}