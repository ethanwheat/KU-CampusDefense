// AbilitiesPanelController.cs
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesPanelController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject abilitiesPanel; // The panel that contains ability buttons
    [SerializeField] private Button mainAbilityButton; // The "Use Abilities" button

    [Header("Prefabs")]
    [SerializeField] private GameObject abilityButtonPrefab;
    [SerializeField] private Transform buttonsParent; // Where ability buttons will be placed

    [Header("Abilities")]
    [SerializeField] private SlowAllEnemiesAbility slowAllEnemiesAbility;

    private void Start()
    {
        // Initialize the UI
        InitializeAbilities();
        
        // Set up main button click
        mainAbilityButton.onClick.AddListener(ToggleAbilitiesPanel);
        
        // Hide panel by default
        abilitiesPanel.SetActive(false);
    }

    private void InitializeAbilities()
    {
        // Clear existing buttons (if any)
        foreach (Transform child in buttonsParent)
        {
            Destroy(child.gameObject);
        }
        
        // Create buttons for all abilities
        if (slowAllEnemiesAbility != null) CreateAbilityButton(slowAllEnemiesAbility);
        //if (freezeEnemiesAbility != null) CreateAbilityButton(freezeEnemiesAbility);
        
        // Future abilities would be added here in the same way
    }

    private void CreateAbilityButton(AbilityController abilityController)
    {
        GameObject buttonObj = Instantiate(abilityButtonPrefab, buttonsParent);
        var buttonController = buttonObj.GetComponent<AbilityButtonController>();
        buttonController.Initialize(abilityController);
        
        // When ability is clicked, hide the panel
        buttonController.GetComponent<Button>().onClick.AddListener(() => {
            abilitiesPanel.SetActive(false);
        });
    }

    public void ToggleAbilitiesPanel()
    {
        abilitiesPanel.SetActive(!abilitiesPanel.activeSelf);
    }

    public void CloseAbilitiesPanel()
    {
        abilitiesPanel.SetActive(false);
    }
}