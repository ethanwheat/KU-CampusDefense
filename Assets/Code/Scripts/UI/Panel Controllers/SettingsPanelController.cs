using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectsVolumeSlider;

    [Header("Settings Data Controller")]
    [SerializeField] private SettingsDataController settingsDataController;

    private SoundManager soundManager;

    void Awake()
    {
        soundManager = SoundManager.instance;
    }

    void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener(soundManager.onMusicVolumeChange);
        soundEffectsVolumeSlider.onValueChanged.AddListener(settingsDataController.setSoundEffectsVolume);
    }

    // Update panel with correct music volume and sound effect volume.
    public void updatePanel()
    {
        float musicVolume = settingsDataController.getMusicVolume();
        float soundEffectsVolume = settingsDataController.getSoundEffectsVolume();

        musicVolumeSlider.value = musicVolume;
        soundEffectsVolumeSlider.value = soundEffectsVolume;
    }
}
