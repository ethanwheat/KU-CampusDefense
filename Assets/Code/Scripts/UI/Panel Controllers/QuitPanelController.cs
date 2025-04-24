using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitPanelController : MonoBehaviour
{
    [Header("UI Controllers")]
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    // Quit to main menu.
    public void QuitToMainMenuClick()
    {
        StartCoroutine(QuitToMainMenuCoroutine());
    }

    // Quit to main menu coroutine.
    private IEnumerator QuitToMainMenuCoroutine()
    {
        yield return StartCoroutine(loadingBackgroundController.FadeInCoroutine(.5f));
        SceneManager.LoadScene("Main Menu Scene");
    }

    // Quit game.
    public void QuitGameClick()
    {
        Application.Quit();
    }
}
