using TMPro;
using UnityEngine;

public class MessagePopupPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI messageText;

    // Show message popup panel and set message popup panel data.
    public void showPanel(string title, string message)
    {
        headerText.text = title;
        messageText.text = message;

        gameObject.SetActive(true);
    }

    // Close message popup panel.
    public void closePanel()
    {
        gameObject.SetActive(false);
    }
}
