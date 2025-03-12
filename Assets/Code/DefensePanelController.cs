using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefensePanelController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject defensePlacementButtonPrefab;

    [Header("UI Controllers")]
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("UI Transforms")]
    [SerializeField] private Transform placementButtonsParent;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private DefenseData[] defenseData;
    private GameObject selectedPlacementButton;
    private GameObject currentDefensePlacement;

    void Update()
    {
        // Cancel selection is escape pressed.
        if (Input.GetKeyDown("escape"))
        {
            cancelPlacement();
        }
    }

    // Load defenses in UI.
    public void showPanel()
    {
        // Reset panel.
        resetPanel();

        // Get updated defenses.
        defenseData = gameDataController.getDefenseData();

        for (int i = 0; i < defenseData.Length; i++)
        {
            // Get current defense.
            DefenseData defense = defenseData[i];

            if (defense.isBought())
            {
                // Create defense button and add listeners.
                GameObject placementButton = Instantiate(defensePlacementButtonPrefab, placementButtonsParent);
                placementButton.GetComponent<DefensePlacementButtonController>().setData(defense);
                placementButton.GetComponent<Button>().onClick.AddListener(() => startPlacement(placementButton, defense));

                // Update position on panel.
                Vector3 newPosition = new Vector3(125 + (130 * i), 125, 0);
                placementButton.GetComponent<RectTransform>().anchoredPosition = newPosition;
            }
        }

        // Show panel.
        gameObject.SetActive(true);
    }

    // Close defense panel.
    public void closePanel()
    {
        // Cancel placement, reset panel, hide panel.
        cancelPlacement();
        resetPanel();
        gameObject.SetActive(false);
    }

    // Select a defense in the UI and create placement.
    void startPlacement(GameObject placementButton, DefenseData defense)
    {
        // Reset the defense panel and cancel previous placement.
        cancelPlacement();

        // Set the new placement button.
        selectedPlacementButton = placementButton;

        // Set button to selected.
        selectedPlacementButton.GetComponent<DefensePlacementButtonController>().onSelect();

        // Create and set new placement.
        Transform parentTransform = getPlacementParent().transform;
        currentDefensePlacement = Instantiate(defense.getPrefab(), parentTransform);

        // Set defense data on placement controller and add listener to reset the panel when a placement is made.
        DefencePlacementController placementController = currentDefensePlacement.GetComponent<DefencePlacementController>();
        placementController.setDefenseData(defense);
        placementController.onCancelPlacement.AddListener(cancelPlacement);
    }

    // Cancel placement
    void cancelPlacement()
    {
        // Reset placement button.
        if (selectedPlacementButton)
        {
            selectedPlacementButton.GetComponent<DefensePlacementButtonController>().onDeselect();
        }

        // Cancel placement.
        if (currentDefensePlacement)
        {
            DefencePlacementController placementController = currentDefensePlacement.GetComponent<DefencePlacementController>();

            if (!placementController.isPlaced())
            {
                Destroy(currentDefensePlacement);
            }
        }
    }

    GameObject getPlacementParent()
    {
        // Set the parent of the new defense.
        GameObject parent = null;

        foreach (GameObject currentGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (currentGameObject.name == "Placement")
            {
                parent = currentGameObject;
                break;
            }
        }

        if (!parent)
        {
            parent = new GameObject("Placement");
        }

        return parent;
    }

    void resetPanel()
    {
        // Delete all placement buttons.
        foreach (Transform button in placementButtonsParent)
        {
            Destroy(button.gameObject);
        }
    }
}
