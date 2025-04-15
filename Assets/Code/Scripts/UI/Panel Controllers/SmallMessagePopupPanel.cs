using System.Collections;
using TMPro;
using UnityEngine;

public class SmallMessagePopupPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;

    public void showPanel(string text)
    {
        headerText.text = text;
        gameObject.SetActive(true);
        StartCoroutine(FadeRoutine());
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

