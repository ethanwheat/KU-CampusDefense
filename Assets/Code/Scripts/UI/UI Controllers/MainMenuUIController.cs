using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [Header("UI Controllers")]
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;
    [SerializeField] private PanelFadeController menuPanelFadeController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataObject gameDataController;

    void Start()
    {
        // Call show menu coroutine.
        StartCoroutine(ShowMenu());
    }

    // Show menu.
    IEnumerator ShowMenu()
    {
        yield return StartCoroutine(loadingBackgroundController.FadeOutCoroutine(.5f));
        menuPanelFadeController.Show();
    }

    // Start game.
    public void StartGame()
    {
        // Stop music.
        SoundManager.instance.StopMusic(.5f);

        // Start game coroutine.
        StartCoroutine(StartGameCoroutine());
    }

    // Fade background in, reset game data, and load building scene.
    IEnumerator StartGameCoroutine()
    {
        // Wait until background fades in completly.
        yield return StartCoroutine(loadingBackgroundController.FadeInCoroutine(.5f));

        // Load building scene.
        SceneManager.LoadScene("Building Scene");
    }

    // Called by "Yes" on Quit Confirm
    public void QuitGame()
    {
        Application.Quit();
    }
}