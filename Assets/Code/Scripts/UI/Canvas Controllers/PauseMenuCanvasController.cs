using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenuCanvasController : MonoBehaviour
{
    public static PauseMenuCanvasController instance;

    [Header("Panel Fade Controllers")]
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    [Header("Sounds")]
    [SerializeField] private AudioClip pauseMusic;

    [Header("Unity Events")]
    public UnityEvent OnGamePause;
    public UnityEvent OnGameResume;

    private bool isGamePaused;

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

    void Update()
    {
        // Pause game if escape is pressed.
        if (Input.GetKeyDown("escape"))
        {
            if (isGamePaused)
            {
                ClosePauseMenu();
            }
            else
            {
                ShowPauseMenu();
            }
        }
    }

    // Show pause menu.
    public void ShowPauseMenu()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        OnGamePause.Invoke();
        SoundManager.instance.PlayMusic(pauseMusic, transform);
    }

    // Close pause menu.
    public void ClosePauseMenu()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        OnGameResume.Invoke();
        SoundManager.instance.ResumeMusic();
    }

    // Quit to main menu.
    public void QuitToMainMenu()
    {
        isGamePaused = false;
        SoundManager.instance.StopMusic();
        StartCoroutine(QuitToMainMenuCoroutine());
    }

    // Quit to main menu coroutine.
    private IEnumerator QuitToMainMenuCoroutine()
    {
        yield return StartCoroutine(loadingBackgroundController.FadeInCoroutine(.5f));
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu Scene");
    }
}
