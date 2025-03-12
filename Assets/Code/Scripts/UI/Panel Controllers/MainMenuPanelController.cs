using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPanelController : MonoBehaviour
{
    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private MainMenuUIController mainMenuUIController;

    void Start()
    {
        // Set main menu UI controller.
        mainMenuUIController = transform.parent.GetComponent<MainMenuUIController>();
    }

    // Start game.
    public void startGame()
    {
        // Start game coroutine.
        StartCoroutine(startGameCoroutine());
    }

    // Fade background in, reset game data, and load building scene.
    IEnumerator startGameCoroutine()
    {
        // Wait until background fades in completly.
        yield return StartCoroutine(mainMenuUIController.fadeBackgroundIn());

        // Reset game data.
        gameDataController.resetGameData();

        // Load building scene.
        SceneManager.LoadScene("Gavin's Building Scene");
    }
}
