using System.Collections;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    [Header("UI Controllers")]
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    void Start()
    {
        // Fade background out.
        StartCoroutine(fadeBackgroundOut());
    }

    // Fade background in slowly.
    public IEnumerator fadeBackgroundIn()
    {
        yield return StartCoroutine(loadingBackgroundController.fadeInCoroutine(.5f));
    }

    // Fade background out slowly.
    public IEnumerator fadeBackgroundOut()
    {
        yield return StartCoroutine(loadingBackgroundController.fadeOutCoroutine(.5f));
    }
}
