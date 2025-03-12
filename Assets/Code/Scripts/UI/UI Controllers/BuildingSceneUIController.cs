using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BuildingSceneUIController : MonoBehaviour
{
    [Header("UI Controllers")]
    [SerializeField] private PurchasePanelController purchasePanelController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI dollarText;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private Camera mainCamera;
    private BuildingPlacementController buildingPlacementController;

    void Start()
    {
        // Set main camera.
        mainCamera = Camera.main;

        // Update dollar UI.
        updateDollarUI();

        // Fade out background.
        StartCoroutine(loadingBackgroundController.fadeOutCoroutine(.5f));
    }

    void Update()
    {
        // Create raycast.
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Get building mask.
        int layerMask = LayerMask.GetMask("Building");

        // If raycast hit a building then show correct UI.
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            // Do not allow raycasting through UI.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // Get placementCollider, building placement controller, and object data.
            Collider placementCollider = hit.collider;
            buildingPlacementController = placementCollider.GetComponent<BuildingPlacementController>();
            ObjectData objectData = buildingPlacementController.getObjectData();

            // Only show panels and outlines if building is not locked.
            if (!objectData.isLocked())
            {
                // Show outline.
                buildingPlacementController.showOutline(true);

                // Check if mouse is clicked.
                if (Input.GetMouseButtonDown(0))
                {
                    // Check if object is bought or not.
                    if (objectData.isBought())
                    {
                        // Show defense upgrade panel.
                    }
                    else
                    {
                        // Close existing UI.
                        closeExistingUI();

                        // Show purchase panel with data.
                        purchasePanelController.showPanel(buildingPlacementController, objectData);
                    }
                }
            }
        }
        else
        {
            if (buildingPlacementController)
            {
                buildingPlacementController.showOutline(false);
            }
        }
    }

    // Start round.
    public void startRound()
    {
        StartCoroutine(startRoundCoroutine());
    }

    // Fade background in and load round scene.
    IEnumerator startRoundCoroutine()
    {
        yield return StartCoroutine(loadingBackgroundController.fadeInCoroutine(.5f));

        SceneManager.LoadScene("Round Scene");
    }

    // Update the dollar UI.
    public void updateDollarUI()
    {
        // Update dollar text.
        dollarText.text = gameDataController.getDollarAmount().ToString();
    }

    // Close existing UI.
    void closeExistingUI()
    {
        messagePopupPanelController.closePanel();
        purchasePanelController.closePanel();
    }
}
