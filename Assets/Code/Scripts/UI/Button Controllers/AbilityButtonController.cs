using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AbilityButtonController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TextMeshProUGUI abilityNameText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button abilityButton;
    [SerializeField] private Color effectColor = Color.white;
    [SerializeField] private Color cooldownColor = Color.red;

    private AbilityData abilityData;
    private bool isOnCooldown;
    private bool isEffectActive;

    private void Awake()
    {
        abilityButton.onClick.AddListener(OnAbilityClicked);
        if (timerText != null) timerText.gameObject.SetActive(false);
    }

    public void Initialize(AbilityData data)
    {
        abilityData = data;
        if (abilityIcon != null) abilityIcon.sprite = data.Icon;
        if (abilityNameText != null) abilityNameText.text = data.AbilityName;
        UpdateUI();
    }

    private void OnAbilityClicked()
    {
        if (isOnCooldown || isEffectActive) return;
        
        // Start effect duration countdown
        StartCoroutine(EffectRoutine());
        // Activate ability
        AbilityManager.Instance.ActivateAbility(abilityData);
    }

    private IEnumerator EffectRoutine()
    {
        isEffectActive = true;
        float remainingEffect = abilityData.EffectDuration;
        
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            timerText.color = effectColor;
            
            while (remainingEffect > 0)
            {
                remainingEffect -= Time.deltaTime;
                timerText.text = Mathf.CeilToInt(remainingEffect).ToString();
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(abilityData.EffectDuration);
        }
        
        isEffectActive = false;
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        float remainingCooldown = abilityData.CooldownDuration;
        
        if (timerText != null)
        {
            timerText.color = cooldownColor;
            
            while (remainingCooldown > 0)
            {
                remainingCooldown -= Time.deltaTime;
                timerText.text = Mathf.CeilToInt(remainingCooldown).ToString();
                yield return null;
            }
            
            timerText.gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(abilityData.CooldownDuration);
        }
        
        isOnCooldown = false;
        UpdateUI();
    }

    private void UpdateUI()
    {
        abilityButton.interactable = !isOnCooldown && !isEffectActive;
    }
}