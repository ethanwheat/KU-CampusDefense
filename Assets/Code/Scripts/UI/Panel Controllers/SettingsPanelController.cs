using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectsVolumeSlider;

    [Header("UI Controllers")]
    [SerializeField] private PanelFadeController panelFadeController;

    void Start()
    {
        SoundManager soundManager = SoundManager.instance;

        musicVolumeSlider.onValueChanged.AddListener(soundManager.OnMusicVolumeChange);
        soundEffectsVolumeSlider.onValueChanged.AddListener(soundManager.OnSoundEffectVolumeChange);
    }

    // ShowPanel panel with correct music volume and sound effect volume.
    public void ShowPanel()
    {
        musicVolumeSlider.value = SoundManager.instance.MusicVolume;
        soundEffectsVolumeSlider.value = SoundManager.instance.SoundEffectsVolume;

        panelFadeController.Show();
    }

    public void OnReset()
    {
        musicVolumeSlider.value = 1f;
        soundEffectsVolumeSlider.value = 1f;
    }
}
