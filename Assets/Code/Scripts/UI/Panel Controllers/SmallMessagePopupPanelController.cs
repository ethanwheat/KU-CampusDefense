using System.Collections;
using TMPro;
using UnityEngine;

public class SmallMessagePopupPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;

    private Coroutine fadeCoroutine;

    public void ShowPanel(string text)
    {
        headerText.text = text;
        gameObject.SetActive(true);

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        GetComponent<PanelFadeController>().Show();

        // Stay visible
        yield return new WaitForSeconds(2f);

        // Fade out
        GetComponent<PanelFadeController>().Hide();
    }
}

