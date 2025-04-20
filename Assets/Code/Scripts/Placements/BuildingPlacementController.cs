using UnityEngine;

public class BuildingPlacementController : MonoBehaviour
{
    [Header("Building Information")]
    [SerializeField] private string buildingName;
    [SerializeField] private PurchasableDataObject purchasableData;

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
    [SerializeField] private GameDataObject gameDataController;

    public string BuildingName => buildingName;
    public PurchasableDataObject PurchasableDataObject => purchasableData;
    public bool RoundScene => roundScene;


    private Transform overlays;

    void Start()
    {
        // Show placement area.
        UpdatePlacementArea();
    }

    // Update the placement area.
    public void UpdatePlacementArea()
    {
        // Reset placement area.
        ResetPlacementArea();

        // Store if object is bought and locked.
        bool isBought = purchasableData.Bought;
        bool isLocked = purchasableData.Locked;

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
            CreateOverlay();
        }

        // Set placement material
        SetPlacementMaterial(isLocked);

        // Set placement as active.
        placementGameObject.SetActive(true);

    }

    // Reset placement area.
    void ResetPlacementArea()
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
    void SetPlacementMaterial(bool isLocked)
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
    void CreateOverlay()
    {
        if (!overlays)
        {
            overlays = new GameObject("Overlays").transform;
            overlays.parent = transform;
            overlays.localPosition = new Vector3(0, 1, 0);
        }

        if (purchasableData.Locked)
        {
            GameObject overlay = Instantiate(lockedBuildingOverlayPrefab, overlays);
            overlay.GetComponent<LockedBuildingOverlayController>().SetData(buildingName);
        }
        else
        {
            GameObject overlay = Instantiate(unlockedBuildingOverlayPrefab, overlays);
            overlay.GetComponent<UnlockedBuildingOverlayController>().SetData(buildingName, purchasableData);
        }
    }

    // Show outline on placement.
    public void ShowOutline(bool visible)
    {
        // Show outlines.
        if (purchasableData.Bought)
        {
            buildingGameObject.GetComponent<Outline>().enabled = visible;
        }
        else
        {
            placementGameObject.GetComponent<Outline>().enabled = visible;
        }
    }
}
