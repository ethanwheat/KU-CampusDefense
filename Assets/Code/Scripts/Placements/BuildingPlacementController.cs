using TMPro;
using UnityEngine;

public class BuildingPlacementController : MonoBehaviour
{
    [Header("Building Information")]
    [SerializeField] private string buildingName;
    [SerializeField] private PurchasableData purchasableData;

    [Header("Scene Information")]
    [SerializeField] private bool roundScene;

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

    private Transform overlays;

    void Start()
    {
        // Show placement area.
        updatePlacementArea();
    }

    // Update the placement area.
    public void updatePlacementArea()
    {
        // Reset placement area.
        resetPlacementArea();

        // Store if object is bought and locked.
        bool isBought = purchasableData.isBought();
        bool isLocked = purchasableData.isLocked();

        // Check if defense is bought.
        if (isBought)
        {
            // Show building if defense is bought.
            buildingGameObject.SetActive(true);

            return;
        }

        // Create overlay if not round scene.
        if (!roundScene)
        {
            createOverlay();
        }

        // Set placement material
        setPlacementMaterial(isLocked);

        // Set placement as active.
        placementGameObject.SetActive(true);

    }

    // Reset placement area.
    void resetPlacementArea()
    {
        // Set placement and building to not be active.
        placementGameObject.SetActive(false);
        buildingGameObject.SetActive(false);

        // Delete overlays
        if (overlays)
        {
            Destroy(overlays.gameObject);
        }
    }

    // If defense is unlocked and is not round scene then change color to available material else set to unavailable color.
    void setPlacementMaterial(bool isLocked)
    {
        // Get mesh renderer.
        MeshRenderer meshRenderer = placementGameObject.GetComponent<MeshRenderer>();

        if (!isLocked && !roundScene)
        {
            meshRenderer.material = availableMaterial;
        }
        else
        {
            meshRenderer.material = unavailableMaterial;
        }
    }

    // Create overlay.
    void createOverlay()
    {
        if (!overlays)
        {
            overlays = new GameObject("Overlays").transform;
            overlays.parent = transform;
            overlays.localPosition = new Vector3(0, 1, 0);
        }

        if (purchasableData.isLocked())
        {
            GameObject overlay = Instantiate(lockedBuildingOverlayPrefab, overlays);
            overlay.GetComponent<LockedBuildingOverlayController>().setData(buildingName);
        }
        else
        {
            GameObject overlay = Instantiate(unlockedBuildingOverlayPrefab, overlays);
            overlay.GetComponent<UnlockedBuildingOverlayController>().setData(buildingName, purchasableData);
        }
    }

    // Get building name.
    public string getBuildingName()
    {
        return buildingName;
    }

    // Get object data.
    public PurchasableData getPurchasableData()
    {
        return purchasableData;
    }

    // Show outline on placement.
    public void showOutline(bool visible)
    {
        // Show outlines.
        if (purchasableData.isBought())
        {
            buildingGameObject.GetComponent<Outline>().enabled = visible;
        }
        else
        {
            placementGameObject.GetComponent<Outline>().enabled = visible;
        }
    }

    public bool isRoundScene()
    {
        return roundScene;
    }
}
