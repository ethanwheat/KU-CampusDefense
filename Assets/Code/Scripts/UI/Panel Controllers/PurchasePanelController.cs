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

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private PurchasableData purchasableData;
    private BuildingPlacementController buildingPlacementController;

    // Load purchase panel data.
    public void showPanel(BuildingPlacementController controller, PurchasableData data)
    {
        // Set building name and set object data.
        buildingPlacementController = controller;
        purchasableData = data;

        // Set UI sprite.
        itemImage.sprite = purchasableData.getSprite();

        // Set UI text.
        headerText.text = buildingPlacementController.getBuildingName();
        itemText.text = purchasableData.getName();
        itemDescription.text = purchasableData.getDescription();
        costText.text = purchasableData.getDollarCost().ToString();

        // Show panel
        gameObject.SetActive(true);
    }

    // Set object to bought, subtract dollars, create message popup panel, and update dollar UI
    // if player has enough dollars, else show error popup panel and close purchase panel.
    public void onPurchase()
    {
        // Get dollar amount and building name.
        int dollars = gameDataController.getDollarAmount();
        int objectCost = purchasableData.getDollarCost();
        string buildingName = buildingPlacementController.getBuildingName();

        if (dollars >= objectCost)
        {
            // Set object as bought, subtract cost, create popup panel showing success,
            // update dollar amounts on dollar UI, close purchase panel, and refresh building placement controller.
            purchasableData.setBought(true);
            gameDataController.subtractDollars(objectCost);
            messagePopupPanelController.showPanel("Item Purchased", "You have bought " + buildingName + " for " + objectCost.ToString() + " dollars!");
            buildingSceneUIController.updateDollarUI();
            buildingPlacementController.updatePlacementArea();
            closePanel();
        }
        else
        {
            // Show error popup panel and close purchase panel.
            messagePopupPanelController.showPanel("Insufficient Dollars", "You do not have enough dollars to buy " + buildingName + "!");
            closePanel();
        }
    }

    // Close purchase panel.
    public void closePanel()
    {
        gameObject.SetActive(false);
    }
}
