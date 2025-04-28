using UnityEngine;

public class QuitPanelController : MonoBehaviour
{
    [Header("UI Controllers")]
    [SerializeField] private ConfirmPanelController confirmPanelController;
    [SerializeField] private PanelFadeController panelFadeController;

    // Quit to main menu.
    public void QuitToMainMenuClick()
    {
        confirmPanelController.ShowPanel("Quit to Main Menu", "Are you sure you want to quit to the main menu?", true);
        confirmPanelController.OnConfirm.AddListener(PauseMenuCanvasController.instance.QuitToMainMenu);
        confirmPanelController.OnClose.AddListener(panelFadeController.Show);
    }

    // Quit game.
    public void QuitGameClick()
    {
        confirmPanelController.ShowPanel("Quit Game", "Are you sure you want to quit the game?", true);
        confirmPanelController.OnConfirm.AddListener(Application.Quit);
        confirmPanelController.OnClose.AddListener(panelFadeController.Show);
    }
}
