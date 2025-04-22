using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefensePanelController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject defensePlacementButtonPrefab;

    [Header("UI Controllers")]
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("UI Transforms")]
    [SerializeField] private Transform placementButtonsParent;

    [Header("Sounds")]
    [SerializeField] private AudioClip errorSoundEffect;

    [Header("Game Object")]
    [SerializeField] private GameDataObject gameDataObject;

    private List<DefenseObject> defenseObjects;
    private GameObject selectedPlacementButton;
    private GameObject currentDefensePlacement;

    void Update()
    {
        // Cancel selection is escape pressed.
        if (Input.GetKeyDown("escape"))
        {
            CancelPlacement();
        }
    }

    // Load defenses in UI.
    public void ShowPanel()
    {
        // Destroy current placement.
        DestroyCurrentPlacement();

        // Delete all placement buttons.
        foreach (Transform button in placementButtonsParent)
        {
            Destroy(button.gameObject);
        }

        // Get updated defenses.
        defenseObjects = gameDataObject.DefenseObjects;

        // Set intial index to 0.
        int index = 0;

        foreach (var defense in defenseObjects)
        {
            // Get defense data.
            DefenseData defenseData = GameDataManager.instance.GameData.GetDefenseData(defense.ObjectName);

            if (defenseData != null && defense.IsShownInDefensePanel)
            {
                // Create defense button and add listeners.
                GameObject placementButton = Instantiate(defensePlacementButtonPrefab, placementButtonsParent);
                placementButton.GetComponent<DefensePlacementButtonController>().SetData(defense);
                placementButton.GetComponent<Button>().onClick.AddListener(() => StartPlacement(placementButton, defense));

                // Update index
                index++;
            }
        }

        // Show panel.
        gameObject.SetActive(true);
    }

    // Select a defense in the UI and create placement.
    void StartPlacement(GameObject placementButton, DefenseObject defense)
    {
        // Cancel previous placement.
        CancelPlacement();

        // Set the new placement button.
        selectedPlacementButton = placementButton;

        // Set button to selected.
        selectedPlacementButton.GetComponent<DefensePlacementButtonController>().OnSelect();

        // Create and set new placement.
        currentDefensePlacement = Instantiate(defense.Prefab, RoundManager.instance.PlacementParent);

        // Set defense data on placement controller and add listeners to reset the panel and destroy current placement when a placement is made.
        DefensePlacementController placementController = currentDefensePlacement.GetComponent<DefensePlacementController>();
        placementController.LoadData(defense);
        placementController.onPlacementSuccess.AddListener(PlacementSuccess);
        placementController.onPlacementFail.AddListener(() => PlacementFailed(defense));
    }

    // Set current defense placement to null and reset current placement button.
    void PlacementSuccess()
    {
        currentDefensePlacement = null;
        ResetCurrentPlacementButton();
    }

    // Show error popup message and close panel.
    void PlacementFailed(DefenseObject defenseData)
    {
        SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, 1f);
        messagePopupPanelController.ShowPanel("Insufficient Coins", "You do not have enough coins to buy a " + defenseData.ObjectName + "!");
        ClosePanel();
    }

    // Reset current placement button.
    void ResetCurrentPlacementButton()
    {
        if (selectedPlacementButton)
        {
            selectedPlacementButton.GetComponent<DefensePlacementButtonController>().OnDeselect();
        }
    }

    // Destroys current placement.
    void DestroyCurrentPlacement()
    {
        if (currentDefensePlacement)
        {
            Destroy(currentDefensePlacement);
        }
    }

    // Reset placement button and destroy current placement.
    void CancelPlacement()
    {
        ResetCurrentPlacementButton();
        DestroyCurrentPlacement();
    }

    // Close defense panel.
    public void ClosePanel()
    {
        // Destroy current placement and hide panel.
        DestroyCurrentPlacement();
        gameObject.SetActive(false);
    }
}
