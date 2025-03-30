using System.Collections;
using UnityEngine;

public class PanelFadeController : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn(0.3f));
    }

    public void Hide()
    {
        StartCoroutine(FadeOut(0.3f));
    }

    private IEnumerator FadeIn(float duration)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;

        float timer = 0f;
        while (timer < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeOut(float duration)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float startAlpha = canvasGroup.alpha;
        float timer = 0f;
        while (timer < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
