using TMPro;
using UnityEngine;

public class LockedBuildingOverlayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingText;

    public void setData(string buildingName)
    {
        buildingText.text = buildingName;
    }
}
