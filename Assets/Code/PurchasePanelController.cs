using TMPro;
using UnityEngine;

public class PurchasePanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI costText;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private ObjectData objectData;

    private BuildingSceneUIController buildingSceneUIController;
    private BuildingPlacementController buildingPlacementController;

    public void Start()
    {
        buildingSceneUIController = transform.parent.GetComponent<BuildingSceneUIController>();
    }

    // Load purchase panel data.
    public void loadData(BuildingPlacementController controller, ObjectData data)
    {
        // Set building name and set object data.
        buildingPlacementController = controller;
        objectData = data;

        // Set UI text.
        headerText.text = buildingPlacementController.getBuildingName();
        itemText.text = objectData.getName();
        itemDescription.text = objectData.getDescription();
        costText.text = objectData.getDollarCost().ToString();
    }

    // Set object to bought, subtract dollars, create message popup panel, and update dollar UI
    // if player has enough dollars, else show error popup panel and close purchase panel.
    public void onPurchase()
    {
        // Get dollar amount and building name.
        int dollars = gameDataController.getDollarAmount();
        int objectCost = objectData.getDollarCost();
        string buildingName = buildingPlacementController.getBuildingName();

        if (dollars >= objectCost)
        {
            // Set object as bought, subtract cost, create popup panel showing success,
            // update dollar amounts on dollar UI, close purchase panel, and refresh building placement controller.
            objectData.setBought(true);
            gameDataController.subtractDollars(objectCost);
            buildingSceneUIController.createMessagePopupPanel("Item Purchased", "You have bought " + buildingName + " for " + objectCost.ToString() + " dollars!");
            buildingSceneUIController.updateDollarUI();
            buildingPlacementController.updatePlacementArea();
            closePanel();
        }
        else
        {
            // Show error popup panel and close purchase panel.
            buildingSceneUIController.createMessagePopupPanel("Insufficient Dollars", "You do not have enough dollars to buy " + buildingName + "!");
            closePanel();
        }
    }

    // Close purchase panel.
    public void closePanel()
    {
        Destroy(gameObject);
    }
}
