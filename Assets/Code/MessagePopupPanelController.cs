using TMPro;
using UnityEngine;

public class MessagePopupPanelController : MonoBehaviour
{
    public void setData(string messageTitle, string messageText)
    {
        TextMeshProUGUI title = transform.Find("Header").Find("HeaderText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI message = transform.Find("PanelContent").Find("MessageText").GetComponent<TextMeshProUGUI>();

        title.text = messageTitle;
        message.text = messageText;
    }

    public void closePanel()
    {
        Destroy(gameObject);
    }
}
