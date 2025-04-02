using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedBuildingOverlayController : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI buildingText;
    [SerializeField] private TextMeshProUGUI costText;

    public void setData(string buildingName, PurchasableData purchasableData)
    {
        itemImage.sprite = purchasableData.getSprite();
        buildingText.text = buildingName;
        costText.text = purchasableData.getDollarCost().ToString();
    }
}
