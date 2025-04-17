using UnityEngine;

public class RoundLostPanelController : MonoBehaviour
{
    // Show message popup panel and set message popup panel data.
    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    // Close message popup panel.
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
