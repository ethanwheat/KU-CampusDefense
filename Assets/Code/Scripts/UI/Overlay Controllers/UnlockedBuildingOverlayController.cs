using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedBuildingOverlayController : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI buildingText;
    [SerializeField] private TextMeshProUGUI costText;

    public void SetData(string buildingName, PurchasableData purchasableData)
    {
        itemImage.sprite = purchasableData.Sprite;
        buildingText.text = buildingName;
        costText.text = purchasableData.DollarCost.ToString();
    }
}
