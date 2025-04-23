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
    [SerializeField] private StarImagesController starImagesController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Sounds")]
    [SerializeField] private AudioClip upgradeSoundEffect;
    [SerializeField] private AudioClip errorSoundEffect;

    private string buildingName;
    private DefenseObject defenseObject;
    private DefenseData defenseData;

    // Load purchase panel data.
    public void ShowPanel(string name, DefenseObject defenseObject, DefenseData defenseData)
    {
        // Set building name.
        buildingName = name;

        // Set defense object and defense data.
        this.defenseObject = defenseObject;
        this.defenseData = defenseData;

        // Set UI text.
        headerText.text = buildingName;
        itemImage.sprite = defenseObject.Sprite;
        itemText.text = defenseObject.ObjectName;
        itemDescription.text = defenseObject.Description;

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
            costText.text = defenseObject.GetUpgradeCost(defenseLevel).ToString();
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

        // Get dollar amount, and building name.
        int dollars = gameData.Dollars;
        int upgradeCost = defenseObject.GetUpgradeCost(defenseData.Level);

        if (dollars >= upgradeCost)
        {
            // Upgrade object, subtract cost, update dollar amounts on dollar UI, play upgrade sound, and update upgrade panel.
            defenseData.UpgradeLevel();
            gameData.SubtractDollars(upgradeCost);
            BuildingSceneUIController.instance.UpdateDollarUI();
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
