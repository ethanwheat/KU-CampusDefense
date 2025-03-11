using System.Collections;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject menuPanelPrefab;
    [SerializeField] private GameObject backgroundPrefab;

    private GameObject background;

    void Start()
    {
        // Create menu panel.
        Instantiate(menuPanelPrefab, transform);

        // Fade background out.
        StartCoroutine(fadeBackgroundOut());
    }

    // Fade background in slowly.
    public IEnumerator fadeBackgroundIn()
    {
        if (background)
        {
            Destroy(background);
        }

        background = Instantiate(backgroundPrefab, transform);
        BackgroundController backgroundController = background.GetComponent<BackgroundController>();

        yield return StartCoroutine(backgroundController.fadeInCoroutine(.5f));
    }

    // Fade background out slowly.
    public IEnumerator fadeBackgroundOut()
    {
        background = Instantiate(backgroundPrefab, transform);
        BackgroundController backgroundController = background.GetComponent<BackgroundController>();

        yield return StartCoroutine(backgroundController.fadeOutCoroutine(.5f));

        Destroy(background);

    }
}
