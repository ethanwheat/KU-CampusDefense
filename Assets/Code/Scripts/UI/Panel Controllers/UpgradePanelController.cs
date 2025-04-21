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
    [SerializeField] private GameDataObject gameDataController;

    private string buildingName;
    private DefenseObject defenseData;

    // Load purchase panel data.
    public void ShowPanel(string name, DefenseObject data)
    {
        // Set building name and object data.
        buildingName = name;
        defenseData = data;

        // Set UI text.
        headerText.text = buildingName;
        itemImage.sprite = defenseData.Sprite;
        itemText.text = defenseData.ObjectName;
        itemDescription.text = defenseData.Description;

        // Set stars, level, and cost.
        UpdateUI();

        // Show panel
        gameObject.SetActive(true);
    }

    // Set stars, level, and cost.
    void UpdateUI()
    {
        int defenseLevel = defenseData.Level;

        starImagesController.UpdateStars(defenseLevel);

        itemLevelText.text = "Level " + defenseLevel.ToString();

        if (defenseLevel < 3)
        {
            costText.text = defenseData.GetUpgradeCost().ToString();
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
    public void OnUpgrade()
    {
        // Get game data.
        GameData gameData = GameDataManager.instance.GameData;

        // Get dollar amount and building name.
        int dollars = gameData.Dollars;
        int upgradeCost = defenseData.GetUpgradeCost();

        if (dollars >= upgradeCost)
        {
            // Upgrade object, subtract cost, update dollar amounts on dollar UI, play upgrade sound, and update upgrade panel.
            defenseData.UpgradeLevel();
            gameData.SubtractDollars(upgradeCost);
            buildingSceneUIController.UpdateDollarUI();
            SoundManager.instance.PlaySoundEffect(upgradeSoundEffect, transform, 1f);
            UpdateUI();
        }
        else
        {
            // Show error popup panel and close upgrade panel.
            SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, 1f);
            messagePopupPanelController.ShowPanel("Insufficient Dollars", "You do not have enough dollars to upgrade " + buildingName + "!");
            ClosePanel();
        }
    }

    // Close purchase panel.
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
