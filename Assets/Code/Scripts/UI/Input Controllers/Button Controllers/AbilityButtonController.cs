using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButtonController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI abilityNameText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button abilityButton;
    [SerializeField] private Color effectColor = Color.white;
    [SerializeField] private Color cooldownColor = Color.red;

    [Header("Sounds")]
    [SerializeField] private AudioClip abilityActivatedSoundEffect;
    [SerializeField] private AudioClip errorSoundEffect;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataObject gameDataController;

    private AbilityObject abilityObject;
    private bool isOnCooldown = false;
    private bool isEffectActive = false;
    private AbilitiesPanelController abilitiesPanelController;
    private RoundSceneUIController roundSceneUIController;
    private MessagePopupPanelController messagePopupPanelController;

    private void Awake()
    {
        abilityButton.onClick.AddListener(OnAbilityClicked);
        if (timerText != null) timerText.gameObject.SetActive(false);
    }

    public void Initialize(AbilityObject abilityObject, AbilitiesPanelController abilities, RoundSceneUIController roundSceneUI, MessagePopupPanelController messagePopup)
    {
        this.abilityObject = abilityObject;
        abilitiesPanelController = abilities;
        roundSceneUIController = roundSceneUI;
        messagePopupPanelController = messagePopup;
        if (abilityIcon != null) abilityIcon.sprite = abilityObject.Icon;
        if (costText != null) costText.text = abilityObject.DollarCost.ToString();
        if (abilityNameText != null) abilityNameText.text = abilityObject.AbilityName;
    }

    private void OnAbilityClicked()
    {
        GameData gameData = GameDataManager.instance.GameData;

        if (isEffectActive)
        {
            showError("Already in effect", "The " + abilityObject.AbilityName + " ability is already in effect!");
            return;
        }

        if (isOnCooldown)
        {
            showError("Cooling down", "The " + abilityObject.AbilityName + " ability is cooling down!");
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
            SoundManager.instance.PlaySoundEffect(abilityActivatedSoundEffect, transform, 1f);
            messagePopupPanelController.ShowPanel("Ability activated", "The " + abilityObject.AbilityName + " ability has been activated!");
            abilitiesPanelController.ClosePanel();
        }
        else
        {
            showError("Insufficient Dollars", "You do not have enough dollars to buy the " + abilityObject.AbilityName + " ability!");
        }
    }

    void showError(string title, string text)
    {
        SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, 1f);
        messagePopupPanelController.ShowPanel(title, text);
        abilitiesPanelController.ClosePanel();
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