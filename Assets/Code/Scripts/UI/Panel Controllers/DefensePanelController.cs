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

    [Header("Round Manager")]
    [SerializeField] private RoundManager roundManager;

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
        // Cancel previous placement.
        cancelPlacement();

        // Set the new placement button.
        selectedPlacementButton = placementButton;

        // Set button to selected.
        selectedPlacementButton.GetComponent<DefensePlacementButtonController>().onSelect();

        // Create and set new placement.
        Transform parentTransform = getRootGameObject("Placement").transform;
        currentDefensePlacement = Instantiate(defense.getPrefab(), parentTransform);

        // Set defense data on placement controller and add listener to reset the panel when a placement is made.
        DefensePlacementController placementController = currentDefensePlacement.GetComponent<DefensePlacementController>();
        placementController.loadData(defense, roundManager, messagePopupPanelController);
        placementController.onCancelPlacement.AddListener(cancelPlacement);
    }

    // Cancel placement and reset placement button
    void cancelPlacement()
    {
        // Deselect placement button.
        if (selectedPlacementButton)
        {
            selectedPlacementButton.GetComponent<DefensePlacementButtonController>().onDeselect();
        }

        if (currentDefensePlacement)
        {
            DefensePlacementController defensePlacementController = currentDefensePlacement.GetComponent<DefensePlacementController>();

            // Only delete if not placed.
            if (!defensePlacementController.isPlaced())
            {
                Destroy(currentDefensePlacement);
            }
        }
    }

    // Get root game object
    GameObject getRootGameObject(string name)
    {
        GameObject gameObject = null;

        foreach (GameObject currentGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (currentGameObject.name == name)
            {
                gameObject = currentGameObject;
                break;
            }
        }

        if (!gameObject)
        {
            gameObject = new GameObject(name);
        }

        return gameObject;
    }

    // Reset panel.
    void resetPanel()
    {
        // Delete all placement buttons.
        foreach (Transform button in placementButtonsParent)
        {
            Destroy(button.gameObject);
        }
    }
}
