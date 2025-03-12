using TMPro;
using UnityEngine;

public class UnlockedBuildingOverlayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingText;
    [SerializeField] private TextMeshProUGUI costText;

    public void setData(string buildingName, string cost)
    {
        buildingText.text = buildingName;
        costText.text = cost;
    }
}
