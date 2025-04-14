using TMPro;
using UnityEngine;

public class RoundLostPanelController : MonoBehaviour
{
    // Show message popup panel and set message popup panel data.
    public void showPanel()
    {
        gameObject.SetActive(true);
    }

    // Close message popup panel.
    public void closePanel()
    {
        gameObject.SetActive(false);
    }
}
