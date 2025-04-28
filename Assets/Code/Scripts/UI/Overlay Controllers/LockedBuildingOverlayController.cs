using TMPro;
using UnityEngine;

public class LockedBuildingOverlayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingText;

    public void SetData(string buildingName)
    {
        buildingText.text = buildingName;
    }
}
