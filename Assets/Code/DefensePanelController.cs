using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefensePanelController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject defensePlacementButtonPrefab;

    [Header("Sprites")]
    [SerializeField] private Sprite unselectedPressedSprite;
    [SerializeField] private Sprite unselectedUnpressedSprite;
    [SerializeField] private Sprite selectedPressedSprite;
    [SerializeField] private Sprite selectedUnpressedSprite;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private DefenseData[] defenseData;
    private GameObject selectedPlacementButton;
    private GameObject currentDefensePlacement;

    void Start()
    {
        // Load the defenses.
        loadDefenses();
    }

    void Update()
    {
        // Cancel selection is escape pressed.
        if (Input.GetKeyDown("escape"))
        {
            cancelPlacement();
        }
    }

    // Close defense panel.
    public void closePanel()
    {
        // Destroys the panel and cancels placement.
        cancelPlacement();
        Destroy(gameObject);
    }

    // Load defenses in UI.
    void loadDefenses()
    {
        // Get updated defenses.
        defenseData = gameDataController.getDefenseData();

        for (int i = 0; i < defenseData.Length; i++)
        {
            // Get current defense.
            DefenseData defense = defenseData[i];

            if (defense.isBought())
            {
                // Get placementButtonContainer.
                Transform placementButtonsContainer = transform.Find("PlacementButtons");

                // Create defense button and add listeners.
                GameObject placementButton = Instantiate(defensePlacementButtonPrefab, placementButtonsContainer);
                Transform placementButtonTransform = placementButton.transform;
                placementButton.GetComponent<Button>().onClick.AddListener(() => selectDefense(placementButton, defense));

                // Update position on panel.
                Vector3 newPosition = new Vector3(125, 125, 0);
                newPosition.x += 130 * i;
                placementButton.GetComponent<RectTransform>().anchoredPosition = newPosition;

                // Update image, name, and cost on button.
                Image defenseImage = placementButtonTransform.Find("DefenseImage").GetComponentInChildren<Image>();
                TextMeshProUGUI defenseText = placementButtonTransform.Find("DefenseText").GetComponentInChildren<TextMeshProUGUI>();
                TextMeshProUGUI defenseCostText = placementButtonTransform.Find("Cost/CostText").GetComponentInChildren<TextMeshProUGUI>();

                Sprite sprite = defense.getSprite();

                if (sprite)
                {
                    defenseImage.sprite = sprite;
                }

                defenseText.text = defense.getName();
                defenseCostText.text = defense.getCoinCost().ToString();
            }
        }
    }

    // Select a defense in the UI and create placement.
    void selectDefense(GameObject placementButton, DefenseData defense)
    {
        // Reset the defense panel and cancel previous placement.
        cancelPlacement();

        // Set the new placement button.
        selectedPlacementButton = placementButton;

        // Change current button to be selected.
        Image buttonImage = placementButton.GetComponent<Image>();
        buttonImage.sprite = selectedUnpressedSprite;

        // Reset unpressed button sprite.
        Button selectedButton = selectedPlacementButton.GetComponent<Button>();
        SpriteState spriteState;
        spriteState.pressedSprite = selectedPressedSprite;
        selectedButton.spriteState = spriteState;

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

        Transform parentTransform = parent.transform;

        // Create and set new placement.
        currentDefensePlacement = Instantiate(defense.getPrefab(), parentTransform);

        // Set defense data on placement controller and add listener to reset the panel when a placement is made.
        DefencePlacementController placementController = currentDefensePlacement.GetComponent<DefencePlacementController>();
        placementController.setDefenseData(defense);
        placementController.onCancelPlacement.AddListener(cancelPlacement);
    }

    // Cancel placement
    void cancelPlacement()
    {
        // Reset old button if it exists.
        if (selectedPlacementButton)
        {
            // Reset pressed button sprite.
            Image selectedButtonImage = selectedPlacementButton.GetComponent<Image>();
            selectedButtonImage.sprite = unselectedUnpressedSprite;

            // Reset unpressed button sprite.
            Button selectedButton = selectedPlacementButton.GetComponent<Button>();
            SpriteState spriteState;
            spriteState.pressedSprite = unselectedPressedSprite;
            selectedButton.spriteState = spriteState;
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
}
