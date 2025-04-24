using UnityEngine;
using UnityEngine.Events;

public class PauseMenuCanvasController : MonoBehaviour
{
    public static PauseMenuCanvasController instance;

    [Header("Pause Menu Canvas Settings")]
    [SerializeField] private bool isRoundScene;

    [Header("Panel Fade Controllers")]
    [SerializeField] private PanelFadeController pauseMenuPanelFadeController;

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

    void ShowPauseMenu()
    {
        isGamePaused = true;
        OnGamePause.Invoke();
        pauseMenuPanelFadeController.Show();
    }

    public void ClosePauseMenu()
    {
        isGamePaused = false;
        OnGameResume.Invoke();
    }
}
