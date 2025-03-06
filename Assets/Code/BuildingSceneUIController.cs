using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingSceneUIController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject purchasePanelPrefab;

    private GameObject purchasePanel;

    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask("Building");

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                if (purchasePanel)
                {
                    PurchasePanelController controller = purchasePanel.GetComponent<PurchasePanelController>();
                    controller.closePanel();
                }

                purchasePanel = Instantiate(purchasePanelPrefab, transform);
                PurchasePanelController purchasePanelController = purchasePanel.GetComponent<PurchasePanelController>();
                BuildingPlacementController buildingPlacementController = hit.collider.GetComponent<BuildingPlacementController>();

                string buildingName = buildingPlacementController.getBuildingName();
                ObjectData objectData = buildingPlacementController.getObjectData();

                purchasePanelController.loadData(buildingName, objectData);
            }
        }
    }

    public void startRound()
    {
        // Start round.
    }
}
