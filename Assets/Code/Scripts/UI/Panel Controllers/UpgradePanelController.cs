using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemLevelText;
    [SerializeField] private GameObject upgradeContent;
    [SerializeField] private GameObject fullyUpgradedText;
    [SerializeField] private TextMeshProUGUI costText;

    [Header("UI Controllers")]
    [SerializeField] private BuildingSceneUIController buildingSceneUIController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;
    [SerializeField] private StarImagesController starImagesController;

    [Header("Sounds")]
    [SerializeField] private AudioClip upgradeSoundEffect;
    [SerializeField] private AudioClip errorSoundEffect;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private string buildingName;
    private DefenseData defenseData;

    // Load purchase panel data.
    public void showPanel(string name, DefenseData data)
    {
        // Set building name and object data.
        buildingName = name;
        defenseData = data;

        // Set UI text.
        headerText.text = buildingName;
        itemImage.sprite = defenseData.getSprite();
        itemText.text = defenseData.getName();
        itemDescription.text = defenseData.getDescription();

        // Set stars, level, and cost.
        updateUI();

        // Show panel
        gameObject.SetActive(true);
    }

    // Set stars, level, and cost.
    void updateUI()
    {
        int defenseLevel = defenseData.getLevel();

        starImagesController.updateStars(defenseLevel);

        itemLevelText.text = "Level " + defenseLevel.ToString();

        if (defenseLevel < 3)
        {
            costText.text = defenseData.getUpgradeCost().ToString();
            upgradeContent.SetActive(true);
            fullyUpgradedText.SetActive(false);
        }
        else
        {
            upgradeContent.SetActive(false);
            fullyUpgradedText.SetActive(true);
        }
    }

    // Set object to bought, subtract dollars, create message popup panel, and update dollar UI
    // if player has enough dollars, else show error popup panel and close purchase panel.
    public void onUpgrade()
    {
        // Get dollar amount and building name.
        int dollars = gameDataController.getDollarAmount();
        int upgradeCost = defenseData.getUpgradeCost();

        if (dollars >= upgradeCost)
        {
            // Upgrade object, subtract cost, update dollar amounts on dollar UI, play upgrade sound, and update upgrade panel.
            defenseData.upgradeLevel();
            gameDataController.subtractDollars(upgradeCost);
            buildingSceneUIController.updateDollarUI();
            SoundManager.instance.playSoundEffect(upgradeSoundEffect, transform, 1f);
            updateUI();
        }
        else
        {
            // Show error popup panel and close upgrade panel.
            SoundManager.instance.playSoundEffect(errorSoundEffect, transform, 1f);
            messagePopupPanelController.showPanel("Insufficient Dollars", "You do not have enough dollars to upgrade " + buildingName + "!");
            closePanel();
        }
    }

    // Close purchase panel.
    public void closePanel()
    {
        gameObject.SetActive(false);
    }
}
