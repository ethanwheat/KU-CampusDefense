using TMPro;
using UnityEngine;

public class RegenHealthPanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private GameObject canRegenContent;
    [SerializeField] private GameObject cannotRegenContent;
    [SerializeField] private TextMeshProUGUI costText;

    [Header("UI Controllers")]
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Sounds")]
    [SerializeField] private AudioClip regenSoundEffect;
    [SerializeField] private AudioClip errorSoundEffect;

    // Show health regen panel.
    public void ShowPanel()
    {
        RoundManager roundManager = RoundManager.instance;

        int regenCost = roundManager.GetRegenCost();

        if (regenCost > 0)
        {
            costText.text = regenCost.ToString();
            canRegenContent.SetActive(true);
            cannotRegenContent.SetActive(false);
        }
        else
        {
            cannotRegenContent.SetActive(true);
            canRegenContent.SetActive(false);
        }

        // Show panel.
        gameObject.SetActive(true);
    }

    // Regen health.
    public void RegenHealth()
    {
        RoundManager roundManager = RoundManager.instance;

        int coins = roundManager.Coins;
        int regenCost = roundManager.GetRegenCost();

        if (coins >= regenCost)
        {
            roundManager.RegenHealthOnDefenses();
            SoundManager.instance.PlaySoundEffect(regenSoundEffect, transform, 1f);
            messagePopupPanelController.ShowPanel("Defenses Health Regenerated", "You regenerated health on all defenses for " + regenCost + " coins!");
        }
        else
        {
            SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, 1f);
            messagePopupPanelController.ShowPanel("Insufficient Coins", "You do not have enough coins to regen all defenses!");
        }

        ClosePanel();
    }

    // Close panel.
    public void ClosePanel()
    {
        // Hide panel.
        gameObject.SetActive(false);
    }
}
