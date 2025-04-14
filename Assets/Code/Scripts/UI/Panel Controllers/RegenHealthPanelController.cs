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

    [Header("Round Manager")]
    [SerializeField] private RoundManager roundManager;

    // Show health regen panel.
    public void showPanel()
    {
        int regenCost = roundManager.getRegenCost();

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
    public void regenHealth()
    {
        int coins = roundManager.getCoinAmount();
        int regenCost = roundManager.getRegenCost();

        if (coins >= regenCost)
        {
            roundManager.regenHealthOnDefenses();
            SoundManager.instance.playSoundEffect(regenSoundEffect, transform, 1f);
            messagePopupPanelController.showPanel("Defenses Health Regenerated", "You regenerated health on all defenses for " + regenCost + " coins!");
        }
        else
        {
            SoundManager.instance.playSoundEffect(errorSoundEffect, transform, 1f);
            messagePopupPanelController.showPanel("Insufficient Coins", "You do not have enough coins to regen all defenses!");
        }

        closePanel();
    }

    // Close panel.
    public void closePanel()
    {
        // Hide panel.
        gameObject.SetActive(false);
    }
}
