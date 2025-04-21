using TMPro;
using UnityEngine;

public class CreateGameSavePanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI gameSaveNameInput;

    [Header("UI Controllers")]
    [SerializeField] private MainMenuUIController mainMenuUIController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    public void CreateGameSave()
    {
        GameData gameData = GameSaveController.CreateGameSave(gameSaveNameInput.text);

        if (gameData == null)
        {
            messagePopupPanelController.ShowPanel("Something went wrong!", "The game save could not be created.");
            return;
        }

        mainMenuUIController.StartGame(gameData);
    }
}
