using TMPro;
using UnityEngine;

public class BuildingPlacementController : MonoBehaviour
{
    private enum ObjectDataType
    {
        Defense,
        Bonus
    }

    [Header("Building Information")]
    [SerializeField] private string buildingName;
    [SerializeField] private ObjectDataType objectDataType;
    [SerializeField] private int id;

    [Header("Scene Information")]
    [SerializeField] private bool isRoundScene;

    [Header("Placement Game Objects")]
    [SerializeField] private GameObject placementGameObject;
    [SerializeField] private GameObject buildingGameObject;

    [Header("UI Overlay Prefabs")]
    [SerializeField] private GameObject lockedBuildingOverlayPrefab;
    [SerializeField] private GameObject unlockedBuildingOverlayPrefab;

    [Header("Materials")]
    [SerializeField] private Material availableMaterial;
    [SerializeField] private Material unavailableMaterial;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private ObjectData objectData;
    private bool locked;

    void Start()
    {
        // Show placement area.
        updatePlacementArea();
    }

    // Get builind name.
    public string getBuildingName()
    {
        return buildingName;
    }

    // Get object data.
    public ObjectData getObjectData()
    {
        return objectData;
    }

    // Get if building is locked.
    public bool isLocked()
    {
        return locked;
    }

    // Update the placement area.
    public void updatePlacementArea()
    {
        // Reset placement area.
        resetPlacementArea();

        // Set objectData depending on if objectDataType is defense or bonus.
        if (objectDataType == ObjectDataType.Defense)
        {
            objectData = gameDataController.getDefenseDataById(id);
        }

        if (objectDataType == ObjectDataType.Bonus)
        {
            objectData = gameDataController.getBonusObjectDataById(id);
        }

        // Throw error if invalid defense id.
        if (objectData == null)
        {
            Debug.LogError("No object found in game data controller for Id " + id + ".");

            return;
        }

        // Get cost and if object is bought.
        int cost = objectData.getDollarCost();
        bool isBought = objectData.isBought();

        // Set if the item is locked.
        locked = objectData.getUnlockRound() > gameDataController.getRoundNumber();

        // Check if defense is bought.
        if (isBought)
        {
            // Show building if defense is bought.
            buildingGameObject.SetActive(true);
        }
        else
        {
            // Show placement area if defense is not bought.

            // Get mesh renderer.
            MeshRenderer meshRenderer = placementGameObject.GetComponent<MeshRenderer>();

            if (!isRoundScene)
            {
                if (locked)
                {
                    createLockedBuildingOverlay();
                }
                else
                {
                    createUnlockedBuildingOverlay(cost);
                }
            }

            // If defense is unlocked and is not round scene then change color to available material else set to unavailable color.
            if (!locked && !isRoundScene)
            {
                meshRenderer.material = availableMaterial;
            }
            else
            {
                meshRenderer.material = unavailableMaterial;
            }

            // Set placement as active.
            placementGameObject.SetActive(true);
        }
    }

    // Reset placement area.
    void resetPlacementArea()
    {
        // Set placement and building to not be active.
        placementGameObject.SetActive(false);
        buildingGameObject.SetActive(false);

        // Delete overlays
        Transform overlays = transform.Find("Overlays");

        if (overlays)
        {
            Destroy(overlays.gameObject);
        }
    }

    // Create locked building overlay.
    void createLockedBuildingOverlay()
    {
        // Show overlay.
        Transform overlayParent = getOverlayParent();
        GameObject lockedBuildingOverlay = Instantiate(lockedBuildingOverlayPrefab, overlayParent);

        // Set the text on the overlay to building name.
        Transform overlayTransform = lockedBuildingOverlay.transform;
        TextMeshProUGUI text = overlayTransform.Find("BuildingText").GetComponent<TextMeshProUGUI>();
        text.text = buildingName;
    }

    // Create unlocked building overlay.
    void createUnlockedBuildingOverlay(int cost)
    {
        // Show overlay.
        Transform overlayParent = getOverlayParent();
        GameObject unlockedBuildingOverlay = Instantiate(unlockedBuildingOverlayPrefab, overlayParent);

        // Get the overlay transform
        Transform overlayTransform = unlockedBuildingOverlay.transform;

        // Set the text on the overlay to building name and set the cost on the overlay.
        TextMeshProUGUI buildingText = overlayTransform.Find("BuildingText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI costText = overlayTransform.Find("Cost/CostText").GetComponent<TextMeshProUGUI>();
        buildingText.text = buildingName;
        costText.text = cost.ToString();
    }

    // Get overlay parent.
    Transform getOverlayParent()
    {
        Transform overlays = transform.Find("Overlays");

        if (!overlays)
        {
            overlays = new GameObject("Overlays").transform;
            overlays.parent = transform;
            overlays.localPosition = new Vector3(0, 1, 0);
        }

        return overlays;
    }
}
