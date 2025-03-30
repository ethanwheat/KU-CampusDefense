using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButtonController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image abilityIcon;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private TextMeshProUGUI abilityNameText;
    
    private AbilityController abilityController;
    
    public void Initialize(AbilityController controller)
    {
        abilityController = controller;
        
        // Set UI elements
        abilityIcon.sprite = controller.Data.Icon;
        abilityNameText.text = controller.Data.AbilityName;
        
        // Subscribe to events
        abilityController.OnAbilityActivated.AddListener(OnAbilityActivated);
        abilityController.OnCooldownStarted.AddListener(OnCooldownStarted);
        abilityController.OnCooldownEnded.AddListener(OnCooldownEnded);
        
        GetComponent<Button>().onClick.AddListener(ActivateAbility);
    }
    
    private void ActivateAbility()
    {
        abilityController.ActivateAbility();
    }
    
    private void Update()
    {
        if (abilityController.IsOnCooldown)
        {
            cooldownOverlay.fillAmount = abilityController.CurrentCooldown / abilityController.Data.CooldownTime;
            cooldownText.text = Mathf.CeilToInt(abilityController.CurrentCooldown).ToString();
            cooldownOverlay.gameObject.SetActive(true);
        }
        else
        {
            cooldownOverlay.fillAmount = 0f;
            cooldownText.text = "";
            cooldownOverlay.gameObject.SetActive(false);
        }
    }
    
    private void OnAbilityActivated()
    {
        // Visual feedback when ability is activated
        // animation here ?
    }
    
    private void OnCooldownStarted()
    {
        cooldownOverlay.gameObject.SetActive(true);
    }
    
    private void OnCooldownEnded()
    {
        cooldownOverlay.gameObject.SetActive(false);
    }
}