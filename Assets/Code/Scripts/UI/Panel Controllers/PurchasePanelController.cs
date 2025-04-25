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
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Sounds")]
    [SerializeField] private AudioClip placementSoundEffect;
    [SerializeField] private AudioClip errorSoundEffect;

    private PurchasableObject purchasableObject;
    private BuildingPlacementController buildingPlacementController;

    // Load purchase panel data.
    public void ShowPanel(BuildingPlacementController controller, PurchasableObject purchasableObject)
    {
        // Set building name and set object.
        buildingPlacementController = controller;
        this.purchasableObject = purchasableObject;

        // Set UI sprite.
        itemImage.sprite = purchasableObject.Sprite;

        // Set UI text.
        headerText.text = buildingPlacementController.BuildingName;
        itemText.text = purchasableObject.ObjectName;
        itemDescription.text = purchasableObject.Description;
        costText.text = purchasableObject.DollarCost.ToString();

        // Show panel
        gameObject.SetActive(true);
    }

    // Set object to bought, subtract dollars, create message popup panel, and update dollar UI
    // if player has enough dollars, else show error popup panel and close purchase panel.
    public void OnPurchase()
    {
        // Get game data.
        GameData gameData = GameDataManager.instance.GameData;

        // Get dollar amount and building name.
        int dollars = gameData.Dollars;
        int objectCost = purchasableObject.DollarCost;
        string buildingName = buildingPlacementController.BuildingName;

        if (dollars >= objectCost)
        {
            // Set object as bought, subtract cost, play building placement sound,
            // create popup panel showing success, update dollar amounts on dollar UI, and close purchase panel.
            if (purchasableObject is DefenseObject defenseObject)
            {
                gameData.CreateDefenseData(defenseObject);
            }
            else if (purchasableObject is BonusObject bonusObject)
            {
                gameData.CreateBonusData(bonusObject);
            }

            gameData.SubtractDollars(objectCost);
            SoundManager.instance.PlaySoundEffect(placementSoundEffect, transform, volume: 1f);
            messagePopupPanelController.ShowPanel("Building Purchased", "You have bought " + buildingName + " for " + objectCost.ToString() + " dollars!");
            BuildingSceneUIController.instance.UpdateDollarUI();
            buildingPlacementController.UpdatePlacementArea();
            ClosePanel();
        }
        else
        {
            // Show error popup panel and close purchase panel.
            SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, volume: 1f);
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
