using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MessagePopupPanelController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Unity Events")]
    public UnityEvent OnClose;

    // Show message popup panel and set message popup panel data.
    public void ShowPanel(string title, string message)
    {
        OnClose.RemoveAllListeners();

        headerText.text = title;
        messageText.text = message;

        gameObject.SetActive(true);
    }

    // Close message popup panel.
    public void ClosePanel()
    {
        OnClose.Invoke();
        gameObject.SetActive(false);
    }
}
