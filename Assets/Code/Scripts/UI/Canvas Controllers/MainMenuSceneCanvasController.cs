using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenuSceneCanvasController : MonoBehaviour
{
    public static MainMenuSceneCanvasController instance;

    [Header("UI Controllers")]
    [SerializeField] private PanelFadeController menuPanelFadeController;
    [SerializeField] private ConfirmPanelController confirmPanelController;
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent OnEscape;

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

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (!menuPanelFadeController.gameObject.activeSelf)
            {
                OnEscape.Invoke();
            }
        }
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
        SoundManager.instance.StopMusic();

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
        confirmPanelController.ShowPanel("Quit Game", "Are you sure you want to quit the game?", true);
        confirmPanelController.OnConfirm.AddListener(QuitGame);
        confirmPanelController.OnClose.AddListener(menuPanelFadeController.Show);
    }

    // Called by "Yes" on Quit Confirm
    public void QuitGame()
    {
        Application.Quit();
    }
}