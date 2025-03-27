using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedBuildingOverlayController : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI buildingText;
    [SerializeField] private TextMeshProUGUI costText;

    public void setData(string buildingName, ObjectData objectData)
    {
        itemImage.sprite = objectData.getSprite();
        buildingText.text = buildingName;
        costText.text = objectData.getDollarCost().ToString();
    }
}
