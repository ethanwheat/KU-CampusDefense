using TMPro;
using UnityEngine;

public class CreateGameSavePanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI gameSaveNameInput;

    [Header("UI Controllers")]
    [SerializeField] private MainMenuUIController mainMenuUIController;

    public void CreateGameSave()
    {
        if (GameDataManager.instance.CreateGameData(gameSaveNameInput.text))
        {
            mainMenuUIController.StartGame();
        }
        else
        {
            // Show message popup.
        }
    }
}
