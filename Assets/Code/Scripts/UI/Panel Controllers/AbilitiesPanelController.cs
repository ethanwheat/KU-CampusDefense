using UnityEngine;

public class AbilitiesPanelController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject abilityButtonPrefab;

    [Header("UI Transforms")]
    [SerializeField] private Transform abilitiesParent;

    [Header("UI Controllers")]
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Sounds")]
    [SerializeField] private AudioClip errorSoundEffect;
    [SerializeField] private AudioClip abilityActivatedSoundEffect;

    [Header("Game Data Object")]
    [SerializeField] private GameDataObject gameDataObject;

    private bool alreadyOpened = false;

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        InitializeAbilities();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void ShowError(string title, string text)
    {
        SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, volume: 1f);
        messagePopupPanelController.ShowPanel(title, text);
        ClosePanel();
    }

    public void ShowActivated(string title, string text)
    {
        SoundManager.instance.PlaySoundEffect(abilityActivatedSoundEffect, transform, volume: 1f);
        messagePopupPanelController.ShowPanel(title, text);
        ClosePanel();
    }

    private void InitializeAbilities()
    {
        if (!alreadyOpened)
        {
            alreadyOpened = true;

            foreach (var ability in gameDataObject.AbilityObjects)
            {
                var button = Instantiate(abilityButtonPrefab, abilitiesParent);
                button.name = "Btn_" + ability.AbilityName; // Unique name

                var controller = button.GetComponent<AbilityButtonController>();
                if (controller != null)
                {
                    controller.Initialize(ability, this);
                }

                // Force visible
                button.SetActive(true);
            }
        }
    }
}