using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButtonController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject lockedContent;
    [SerializeField] private GameObject unlockedContent;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI unlockedAbilityNameText;
    [SerializeField] private TextMeshProUGUI lockedAbilityNameText;
    [SerializeField] private TextMeshProUGUI lockedText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button abilityButton;
    [SerializeField] private Color effectColor = Color.white;
    [SerializeField] private Color cooldownColor = Color.red;

    private AbilityObject abilityObject;
    private bool isOnCooldown = false;
    private bool isEffectActive = false;
    private AbilitiesPanelController abilitiesPanelController;
    private RoundSceneCanvasController roundSceneUIController;

    private bool isLocked;

    private void Awake()
    {
        abilityButton.onClick.AddListener(OnAbilityClicked);
        if (timerText != null) timerText.gameObject.SetActive(false);
    }

    public void Initialize(AbilityObject abilityObject, AbilitiesPanelController abilities)
    {
        this.abilityObject = abilityObject;
        abilitiesPanelController = abilities;
        roundSceneUIController = RoundSceneCanvasController.instance;
        isLocked = abilityObject.UnlockRound > GameDataManager.instance.GameData.RoundNumber;

        if (isLocked)
        {
            lockedContent.SetActive(true);
            lockedAbilityNameText.text = abilityObject.AbilityName;
            lockedText.text = "Unlocks at round " + abilityObject.UnlockRound.ToString();

            return;
        }

        unlockedContent.SetActive(true);
        abilityIcon.sprite = abilityObject.Icon;
        costText.text = abilityObject.DollarCost.ToString();
        unlockedAbilityNameText.text = abilityObject.AbilityName;
    }

    private void OnAbilityClicked()
    {
        GameData gameData = GameDataManager.instance.GameData;

        if (isLocked)
        {
            abilitiesPanelController.ShowError("Ability Locked", "The " + abilityObject.AbilityName + " ability will unlock at round " + abilityObject.UnlockRound.ToString() + "!");
            return;
        }

        if (isEffectActive)
        {
            abilitiesPanelController.ShowError("Already in effect", "The " + abilityObject.AbilityName + " ability is already in effect!");
            return;
        }

        if (isOnCooldown)
        {
            abilitiesPanelController.ShowError("Cooling down", "The " + abilityObject.AbilityName + " ability is cooling down!");
            return;
        }

        int dollarCost = abilityObject.DollarCost;

        if (gameData.Dollars >= dollarCost)
        {
            // Set ability manager.
            AbilityManager abilityManager = AbilityManager.instance;

            // Subtract dollars.
            gameData.SubtractDollars(dollarCost);
            roundSceneUIController.UpdateDollarUI();

            // Start effect duration countdown
            abilityManager.StartEffectRoutine(this, abilityObject);
            // Activate ability
            abilityManager.ActivateAbility(abilityObject);

            // Show message.
            abilitiesPanelController.ShowActivated("Ability activated", "The " + abilityObject.AbilityName + " ability has been activated!");
        }
        else
        {
            abilitiesPanelController.ShowError("Insufficient Dollars", "You do not have enough dollars to buy the " + abilityObject.AbilityName + " ability!");
        }
    }

    public void ShowTimer(bool isShowing)
    {
        timerText.gameObject.SetActive(isShowing);
    }

    public void SetTimerEffectColor()
    {
        timerText.color = effectColor;
    }

    public void SetTimerCooldownColor()
    {
        timerText.color = cooldownColor;
    }

    public void SetTimerText(string text)
    {
        timerText.text = text;
    }

    public void setIsEffectActive(bool active)
    {
        isEffectActive = active;
    }

    public void setIsOnCooldown(bool active)
    {
        isOnCooldown = active;
    }
}