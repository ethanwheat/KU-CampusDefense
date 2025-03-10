using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingSceneUIController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject purchasePanelPrefab;
    [SerializeField] private GameObject messagePopupPanelPrefab;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private GameObject purchasePanel;
    private GameObject messagePopupPanel;
    private Camera mainCamera;

    void Start()
    {
        // Set main camera.
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if mouse is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            // Do not allow raycasting through UI.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // Create raycast.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Get building mask.
            int layerMask = LayerMask.GetMask("Building");

            // If raycast hit a building then show correct UI.
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                // Get building placement controller.
                BuildingPlacementController buildingPlacementController = hit.collider.GetComponent<BuildingPlacementController>();

                // Only show panel if building is not locked.
                if (!buildingPlacementController.isLocked())
                {
                    // Get building name.
                    string buildingName = buildingPlacementController.getBuildingName();

                    // Get object data.
                    ObjectData objectData = buildingPlacementController.getObjectData();

                    // Check if object is bought or not.
                    if (objectData.isBought())
                    {
                        // Show defense upgrade panel.
                    }
                    else
                    {
                        // Close existing UI.
                        closeExistingUI();

                        // Create purchase panel.
                        purchasePanel = Instantiate(purchasePanelPrefab, transform);

                        // Load purchase panel with data.
                        PurchasePanelController purchasePanelController = purchasePanel.GetComponent<PurchasePanelController>();
                        purchasePanelController.loadData(buildingName, objectData);
                    }
                }
            }
        }
    }

    public void startRound()
    {
        // Start round.
    }

    public GameObject createMessagePopupPanel(string messageTitle, string messageText)
    {
        // Close existing popup panel.
        closePopupPanel();

        // Create message popup panel.
        messagePopupPanel = Instantiate(messagePopupPanelPrefab, transform);
        messagePopupPanel.GetComponent<MessagePopupPanelController>().setData(messageTitle, messageText);

        // Return message popup panel.
        return messagePopupPanel;
    }

    void closePopupPanel()
    {
        // Close popup panel if it exists.
        if (messagePopupPanel)
        {
            Destroy(messagePopupPanel);
        }
    }

    void closePurchasePanel()
    {
        // Close purchase panel if it exists.
        if (purchasePanel)
        {
            PurchasePanelController controller = purchasePanel.GetComponent<PurchasePanelController>();
            controller.closePanel();
        }
    }

    void closeExistingUI()
    {
        // Close all existing UI.
        closePopupPanel();
        closePurchasePanel();
    }
}
