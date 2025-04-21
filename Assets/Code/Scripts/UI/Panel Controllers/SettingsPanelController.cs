using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectsVolumeSlider;

    [Header("Settings Data Controller")]
    [SerializeField] private SettingsObject settingsObject;

    private SoundManager soundManager;

    void Awake()
    {
        soundManager = SoundManager.instance;
    }

    void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener(soundManager.OnMusicVolumeChange);
        soundEffectsVolumeSlider.onValueChanged.AddListener(settingsObject.SetSoundEffectsVolume);
    }

    // Update panel with correct music volume and sound effect volume.
    public void UpdatePanel()
    {
        float musicVolume = settingsObject.MusicVolume;
        float soundEffectsVolume = settingsObject.SoundEffectsVolume;

        musicVolumeSlider.value = musicVolume;
        soundEffectsVolumeSlider.value = soundEffectsVolume;
    }
}
