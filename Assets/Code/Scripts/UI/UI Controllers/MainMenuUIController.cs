using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [Header("UI Controllers")]
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;
    [SerializeField] private PanelFadeController menuPanelFadeController;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    void Start()
    {
        // Call show menu coroutine.
        StartCoroutine(showMenu());
    }

    // Show menu.
    IEnumerator showMenu()
    {
        yield return StartCoroutine(loadingBackgroundController.fadeOutCoroutine(.5f));
        menuPanelFadeController.Show();
    }

    // Start game.
    public void startGame()
    {
        // Stop music.
        SoundManager.instance.stopMusic(.5f);

        // Start game coroutine.
        StartCoroutine(startGameCoroutine());
    }

    // Fade background in, reset game data, and load building scene.
    IEnumerator startGameCoroutine()
    {
        // Wait until background fades in completly.
        yield return StartCoroutine(loadingBackgroundController.fadeInCoroutine(.5f));

        // Reset game data.
        gameDataController.resetGameData();

        // Load building scene.
        SceneManager.LoadScene("Building Scene");
    }

    // Called by "Yes" on Quit Confirm
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit"); // For testing in the editor
    }
}