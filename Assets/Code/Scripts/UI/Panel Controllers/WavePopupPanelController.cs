using System.Collections;
using TMPro;
using UnityEngine;

public class WavePopupPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;

    public void showPanel(string waveNum)
    {
        headerText.text = "Wave " + waveNum;
        gameObject.SetActive(true);
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        Color originalColor = headerText.color;

        // Fade in
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            headerText.color = new Color(originalColor.r, originalColor.g, originalColor.b, t);
            yield return null;
        }

        headerText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        // Stay visible
        yield return new WaitForSeconds(2f);

        // Fade out
        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            headerText.color = new Color(originalColor.r, originalColor.g, originalColor.b, t);
            yield return null;
        }

        headerText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        gameObject.SetActive(false);
    }
}

