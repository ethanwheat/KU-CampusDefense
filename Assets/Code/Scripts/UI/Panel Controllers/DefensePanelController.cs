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
        // Destroy current placement
        destroyCurrentPlacement();

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
        // Destroy current placement, reset panel, and hide panel.
        destroyCurrentPlacement();
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

        // Set defense data on placement controller and add listeners to reset the panel and destroy current placement when a placement is made.
        DefensePlacementController placementController = currentDefensePlacement.GetComponent<DefensePlacementController>();
        placementController.loadData(defense, roundManager, messagePopupPanelController);
        placementController.onPlacementSuccess.AddListener(placementPlaced);
        placementController.onPlacementFail.AddListener(cancelPlacement);
    }

    // Reset current placement button.
    void resetCurrentPlacementButton()
    {
        if (selectedPlacementButton)
        {
            selectedPlacementButton.GetComponent<DefensePlacementButtonController>().onDeselect();
        }
    }

    // Destroys current placement.
    void destroyCurrentPlacement()
    {
        if (currentDefensePlacement)
        {
            Destroy(currentDefensePlacement);
        }
    }

    // Set current defense placement to null and reset current placement button.
    void placementPlaced()
    {
        currentDefensePlacement = null;
        destroyCurrentPlacement();
    }

    // Reset placement button and destroy current placement.
    void cancelPlacement()
    {
        resetCurrentPlacementButton();
        destroyCurrentPlacement();
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
