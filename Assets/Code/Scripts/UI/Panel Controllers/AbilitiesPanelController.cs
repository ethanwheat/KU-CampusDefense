using UnityEngine;

public class AbilitiesPanelController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject abilityButtonPrefab;

    [Header("UI Transforms")]
    [SerializeField] private Transform abilitiesParent;

    [Header("UI Controllers")]
    [SerializeField] private RoundSceneUIController roundSceneUIController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Game Data")]
    [SerializeField] private AbilityData[] abilityData;
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private GameDataController gameDataController;

    public void ShowAbilitiesPanel()
    {
        gameObject.SetActive(true);
        InitializeAbilities();
    }

    public void CloseAbilitiesPanel()
    {
        gameObject.SetActive(false);
    }

    private void InitializeAbilities()
    {
        //Debug.Log($"Initializing abilities. Parent: {abilitiesParent.name}");

        // Clear existing
        foreach (Transform child in abilitiesParent)
        {
            //Debug.Log($"Destroying old button: {child.name}");
            Destroy(child.gameObject);
        }

        foreach (var ability in abilityData)
        {
            //Debug.Log($"Creating button for: {ability.AbilityName}");

            var button = Instantiate(abilityButtonPrefab, abilitiesParent);
            button.name = "Btn_" + ability.AbilityName; // Unique name

            //Debug.Log($"Button created: {button.name} Active: {button.activeSelf}");

            var controller = button.GetComponent<AbilityButtonController>();
            if (controller != null)
            {
                controller.Initialize(ability, this, roundSceneUIController, messagePopupPanelController);
                //Debug.Log($"Controller initialized for {ability.AbilityName}");
            }
            else
            {
                //Debug.LogError($"No controller on button prefab!");
            }

            // Force visible
            button.SetActive(true);
        }
    }
}