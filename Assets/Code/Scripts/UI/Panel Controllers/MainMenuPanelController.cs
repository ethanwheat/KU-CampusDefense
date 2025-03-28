using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Panel Controllers")]
    public PanelFadeController currentSavedGamesPanelFade;
    public PanelFadeController settingsPanelFade;
    public PanelFadeController aboutPanelFade;
    public PanelFadeController quitConfirmPanelFade;

    // Called by Play Game button
    public void ShowCurrentSavedGamesPanel()
    {
        currentSavedGamesPanelFade.Show();
    }

    public void HideCurrentSavedGamesPanel()
    {
        currentSavedGamesPanelFade.Hide();
    }

    // Called by Settings button
    public void ShowSettingsPanel()
    {
        settingsPanelFade.Show();
    }

    public void HideSettingsPanel()
    {
        settingsPanelFade.Hide();
    }

    // Called by About button
    public void ShowAboutPanel()
    {
        aboutPanelFade.Show();
    }

    public void HideAboutPanel()
    {
        aboutPanelFade.Hide();
    }

    // Called by Quit button
    public void ShowQuitConfirmPanel()
    {
        quitConfirmPanelFade.Show();
    }

    public void HideQuitConfirmPanel()
    {
        quitConfirmPanelFade.Hide();
    }

    // Called by "Yes" on Quit Confirm
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit"); // For testing in the editor
    }
}
