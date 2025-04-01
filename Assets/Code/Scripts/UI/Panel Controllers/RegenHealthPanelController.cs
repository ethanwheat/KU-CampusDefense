using TMPro;
using UnityEngine;

public class RegenHealthPanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI costText;

    // Show health regen panel.
    public void showPanel(int cost)
    {
        costText.text = cost.ToString();

        // Show panel
        gameObject.SetActive(true);
    }
}
