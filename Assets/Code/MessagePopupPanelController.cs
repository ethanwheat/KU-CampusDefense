using TMPro;
using UnityEngine;

public class MessagePopupPanelController : MonoBehaviour
{
    // Set message popup panel data.
    public void setData(string messageTitle, string messageText)
    {
        TextMeshProUGUI title = transform.Find("Header").Find("HeaderText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI message = transform.Find("PanelContent").Find("MessageText").GetComponent<TextMeshProUGUI>();

        title.text = messageTitle;
        message.text = messageText;
    }

    // Close message popup panel.
    public void closePanel()
    {
        Destroy(gameObject);
    }
}
