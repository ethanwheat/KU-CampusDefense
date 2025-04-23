using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneUIController : MonoBehaviour
{

    public static MainMenuSceneUIController instance;

    [Header("UI Controllers")]
    [SerializeField] private PanelFadeController menuPanelFadeController;
    [SerializeField] private ConfirmPanelController quitConfirmPanelController;
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    // Show quit game panel.
    public void ShowQuitConfirmPanel()
    {
        quitConfirmPanelController.OnConfirm.AddListener(QuitGame);
        quitConfirmPanelController.OnQuit.AddListener(menuPanelFadeController.Show);
        quitConfirmPanelController.ShowPanel("Quit Game", "Are you sure you want to quit the game?", true);
    }

    // Called by "Yes" on Quit Confirm
    public void QuitGame()
    {
        Application.Quit();
    }
}