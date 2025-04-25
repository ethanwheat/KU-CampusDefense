using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateGameSavePanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TMP_InputField gameSaveNameInputField;

    [Header("UI Controllers")]
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Sounds")]
    [SerializeField] private AudioClip errorSoundEffect;

    public void CreateGameSave()
    {
        if (GameDataManager.instance.CreateGameData(gameSaveNameInputField.text))
        {
            MainMenuSceneCanvasController.instance.StartGame();
        }
        else
        {
            SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, volume: 1f);
            messagePopupPanelController.ShowPanel("Something went wrong!", "Game save could not be created.");
        }
    }
}
