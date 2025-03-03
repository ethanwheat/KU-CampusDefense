using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DefensePanelUIController : MonoBehaviour
{
    [SerializeField] private GameObject defensePlacementButtonPrefab;
    [SerializeField] private DefenseDataController defenseDataController;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform placementButtonsContainer;

    private DefenseData[] defenseData;
    private GameObject selectedPlacementButton;
    private GameObject currentDefensePlacement;

    void Start()
    {
        // Add listener to destroy panel when closed.
        closeButton.onClick.AddListener(destroyPanel);

        // Load the defenses.
        loadDefenses();
    }

    void loadDefenses()
    {
        // Get updated defenses.
        defenseData = defenseDataController.getData();

        for (int i = 0; i < defenseData.Length; i++)
        {
            // Get current defense.
            DefenseData defense = defenseData[i];
            string defenseName = defense.name;
            GameObject defensePrefab = defense.prefab;

            // Create defense button and add listeners.
            GameObject newPlacementButtonGameObject = Instantiate(defensePlacementButtonPrefab, placementButtonsContainer);
            Button newPlacementButton = newPlacementButtonGameObject.GetComponent<Button>();
            newPlacementButton.onClick.AddListener(() => selectDefense(newPlacementButtonGameObject, defensePrefab));

            // Update position on panel.
            RectTransform newPlacementButtonTransform = newPlacementButtonGameObject.GetComponent<RectTransform>();
            Vector2 newPosition = newPlacementButtonTransform.anchoredPosition;
            newPosition.x += 130 * i;
            newPlacementButtonTransform.anchoredPosition = newPosition;

            // Update text on button.
            TextMeshProUGUI buttonText = newPlacementButtonGameObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = defenseName;
        }
    }

    void selectDefense(GameObject placementButton, GameObject defensePrefab)
    {
        // Reset the defense panel and cancel previous placement.
        resetPanel();
        cancelPlacement();

        // Change current button to be selected.
        Image buttonImage = placementButton.GetComponent<Image>();
        buttonImage.color = Color.green;

        // Set the new placement button.
        selectedPlacementButton = placementButton;

        // Set the parent of the new defense.
        GameObject parent = GameObject.Find("Placement");

        if (!parent)
        {
            parent = new GameObject("Placement");
        }

        Transform parentTransform = parent.transform;

        // Create and set new placement.
        currentDefensePlacement = Instantiate(defensePrefab, parentTransform);

        // Add listener to reset the panel when a placement is made.
        PlacementController placementController = currentDefensePlacement.GetComponent<PlacementController>();
        placementController.onDefensePlace.AddListener(resetPanel);
    }

    // Resets the defense panel
    public void resetPanel()
    {
        // Reset old button if it exists.
        if (selectedPlacementButton)
        {
            Image selectedButtonImage = selectedPlacementButton.GetComponent<Image>();
            selectedButtonImage.color = Color.white;
        }
    }

    // Destroys the current placement.
    void cancelPlacement()
    {
        // Reset old placement.
        if (currentDefensePlacement)
        {
            // Get placementController.
            PlacementController placementController = currentDefensePlacement.GetComponent<PlacementController>();

            // Only delete if defense is not placed.
            if (!placementController.isPlaced)
            {
                placementController.cancelPlacement();
            }
        }
    }

    // Destroys the panel and cancels placement.
    void destroyPanel()
    {
        cancelPlacement();
        Destroy(gameObject);
    }
}
