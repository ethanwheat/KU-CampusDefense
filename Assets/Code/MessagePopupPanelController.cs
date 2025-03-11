using TMPro;
using UnityEngine;

public class MessagePopupPanelController : MonoBehaviour
{
    // Show message popup panel and set message popup panel data.
    public void showPanel(string messageTitle, string messageText)
    {
        TextMeshProUGUI title = transform.Find("Header").Find("HeaderText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI message = transform.Find("PanelContent").Find("MessageText").GetComponent<TextMeshProUGUI>();

        title.text = messageTitle;
        message.text = messageText;

        gameObject.SetActive(true);
    }

    // Close message popup panel.
    public void closePanel()
    {
        gameObject.SetActive(false);
    }
}
