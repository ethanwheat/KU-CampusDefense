using UnityEngine;

public class BuildingPlacementController : MonoBehaviour
{
    [Header("Building Information")]
    [SerializeField] private string buildingName;
    [SerializeField] private PurchasableObject purchasableObject;

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
    public PurchasableObject PurchasableObject => purchasableObject;
    public bool RoundScene => roundScene;

    private DefenseData defenseData;
    private Transform overlays;

    void Start()
    {
        // Set defense data.
        defenseData = GameDataManager.instance.GameData.GetDefenseData(purchasableObject.ObjectName);

        // Show placement area.
        UpdatePlacementArea();
    }

    // Update the placement area.
    public void UpdatePlacementArea()
    {
        // Reset placement area.
        ResetPlacementArea();

        // Check if defense is bought.
        if (defenseData.Bought)
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
    void SetPlacementMaterial()
    {
        // Get mesh renderer.
        MeshRenderer meshRenderer = placementGameObject.GetComponent<MeshRenderer>();

        if (defenseData != null && !roundScene)
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

        if (defenseData != null)
        {
            GameObject overlay = Instantiate(lockedBuildingOverlayPrefab, overlays);
            overlay.GetComponent<LockedBuildingOverlayController>().SetData(buildingName);
        }
        else
        {
            GameObject overlay = Instantiate(unlockedBuildingOverlayPrefab, overlays);
            overlay.GetComponent<UnlockedBuildingOverlayController>().SetData(buildingName, purchasableObject);
        }
    }

    // Show outline on placement.
    public void ShowOutline(bool visible)
    {
        // Show outlines.
        if (defenseData.Bought)
        {
            buildingGameObject.GetComponent<Outline>().enabled = visible;
        }
        else
        {
            placementGameObject.GetComponent<Outline>().enabled = visible;
        }
    }
}
