using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBackgroundController : MonoBehaviour
{
    // Fade background in slowly.
    public IEnumerator fadeInCoroutine(float duration)
    {
        gameObject.SetActive(true);

        Image image = gameObject.GetComponent<Image>();

        float timeElapsed = 0;
        Color startColor = new Color(image.color.r, image.color.g, image.color.b, 0f);
        Color endColor = new Color(image.color.r, image.color.g, image.color.b, 1f);

        image.color = startColor;

        while (timeElapsed < duration)
        {
            // Interpolate between the start and end colors
            image.color = Color.Lerp(startColor, endColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure it's fully opaque after the fade
        image.color = endColor;
    }

    // Fade background out slowly.
    public IEnumerator fadeOutCoroutine(float duration)
    {
        gameObject.SetActive(true);

        Image image = gameObject.GetComponent<Image>();

        float timeElapsed = 0;
        Color startColor = new Color(image.color.r, image.color.g, image.color.b, 1f);
        Color endColor = new Color(image.color.r, image.color.g, image.color.b, 0f);

        image.color = startColor;

        while (timeElapsed < duration)
        {
            // Interpolate between the start and end colors
            image.color = Color.Lerp(startColor, endColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure it's fully opaque after the fade
        image.color = endColor;

        gameObject.SetActive(false);
    }
}
