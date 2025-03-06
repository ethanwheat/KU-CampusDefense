using TMPro;
using UnityEditor.Build.Reporting;
using UnityEngine;

public enum BuildingType
{
    Defense,
    Dorm
}

public class BuildingPlacementController : MonoBehaviour
{
    [Header("Building Information")]
    [SerializeField] private string buildingName;
    [SerializeField] private BuildingType buildingType;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Show placement area.
        showPlacementArea();
    }

    ObjectData getData()
    {
        ObjectData[] objectData = null;

        if (buildingType == BuildingType.Defense)
        {
            objectData = gameDataController.getDefenseData();
        }

        if (buildingType == BuildingType.Dorm)
        {
            objectData = gameDataController.getDormData();
        }

        // Set defesense to current defense.
        foreach (var data in objectData)
        {
            int dataId = data.getId();

            if (dataId == id)
            {
                return data;
            }
        }

        return null;
    }

    void showPlacementArea()
    {
        resetPlacementArea();

        ObjectData objectData = getData();

        // Throw error if invalid defense id.
        if (objectData == null)
        {
            Debug.LogError("No object found in game data controller for Id " + id + ".");

            return;
        }

        int cost = objectData.getBuildingCurrencyCost();
        bool isBought = objectData.isBought();
        bool isUnlocked = objectData.getUnlockRound() <= gameDataController.getRoundNumber();

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
                if (isUnlocked)
                {
                    createUnlockedBuildingOverlay(cost);
                }
                else
                {
                    createLockedBuildingOverlay();
                }
            }

            // If defense is unlocked and is not round scene then change color to available material else set to unavailable color.
            if (isUnlocked && !isRoundScene)
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

    void createUnlockedBuildingOverlay(int cost)
    {
        // Show overlay.
        Transform overlayParent = getOverlayParent();
        GameObject unlockedBuildingOverlay = Instantiate(unlockedBuildingOverlayPrefab, overlayParent);

        // Get the overlay transform
        Transform overlayTransform = unlockedBuildingOverlay.transform;

        // Set the text on the overlay to building name.
        TextMeshProUGUI buildingText = overlayTransform.Find("BuildingText").GetComponent<TextMeshProUGUI>();
        buildingText.text = buildingName;

        // Set the cost on the overlay.
        TextMeshProUGUI costText = overlayTransform.Find("Cost/CostText").GetComponent<TextMeshProUGUI>();
        costText.text = cost.ToString();
    }

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

    // Update is called once per frame
    void Update()
    {

    }
}
