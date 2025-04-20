using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI costText;

    [Header("UI Controllers")]
    [SerializeField] private BuildingSceneUIController buildingSceneUIController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Sounds")]
    [SerializeField] private AudioClip placementSoundEffect;
    [SerializeField] private AudioClip errorSoundEffect;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private PurchasableData purchasableData;
    private BuildingPlacementController buildingPlacementController;

    // Load purchase panel data.
    public void ShowPanel(BuildingPlacementController controller, PurchasableData data)
    {
        // Set building name and set object data.
        buildingPlacementController = controller;
        purchasableData = data;

        // Set UI sprite.
        itemImage.sprite = purchasableData.Sprite;

        // Set UI text.
        headerText.text = buildingPlacementController.BuildingName;
        itemText.text = purchasableData.ObjectName;
        itemDescription.text = purchasableData.Description;
        costText.text = purchasableData.DollarCost.ToString();

        // Show panel
        gameObject.SetActive(true);
    }

    // Set object to bought, subtract dollars, create message popup panel, and update dollar UI
    // if player has enough dollars, else show error popup panel and close purchase panel.
    public void OnPurchase()
    {
        // Get dollar amount and building name.
        int dollars = gameDataController.Dollars;
        int objectCost = purchasableData.DollarCost;
        string buildingName = buildingPlacementController.BuildingName;

        if (dollars >= objectCost)
        {
            // Set object as bought, subtract cost, play building placement sound,
            // create popup panel showing success, update dollar amounts on dollar UI, and close purchase panel.
            purchasableData.SetBought(true);
            gameDataController.SubtractDollars(objectCost);
            SoundManager.instance.PlaySoundEffect(placementSoundEffect, transform, 1f);
            messagePopupPanelController.ShowPanel("Building Purchased", "You have bought " + buildingName + " for " + objectCost.ToString() + " dollars!");
            buildingSceneUIController.UpdateDollarUI();
            buildingPlacementController.UpdatePlacementArea();
            ClosePanel();
        }
        else
        {
            // Show error popup panel and close purchase panel.
            SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, 1f);
            messagePopupPanelController.ShowPanel("Insufficient Dollars", "You do not have enough dollars to buy " + buildingName + "!");
            ClosePanel();
        }
    }

    // Close purchase panel.
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
