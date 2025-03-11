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

    [Header("UI Game Objects")]
    [SerializeField] private GameObject dollarUI;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private Camera mainCamera;
    private Outline outline;

    void Start()
    {
        // Set main camera.
        mainCamera = Camera.main;

        // Update dollar UI.
        updateDollarUI();

        // Fade out background.
        StartCoroutine(fadeBackgroundOut());
    }

    void Update()
    {
        checkHoverOverBuilding();
    }

    // Start round.
    public void startRound()
    {
        StartCoroutine(startRoundCoroutine());
    }

    // Create message pop up panel.
    public void createMessagePopupPanel(string messageTitle, string messageText)
    {
        // Create message popup panel.
        messagePopupPanelController.showPanel(messageTitle, messageText);
    }

    // Update the dollar UI.
    public void updateDollarUI()
    {
        // Update dollar text.
        TextMeshProUGUI dollarText = dollarUI.transform.Find("DollarText").GetComponent<TextMeshProUGUI>();
        dollarText.text = gameDataController.getDollarAmount().ToString();
    }

    // Check if mouse is hovering over building.
    void checkHoverOverBuilding()
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

            // Get placement collider and transform.
            Collider placementCollider = hit.collider;

            // Get building placement controller.
            BuildingPlacementController buildingPlacementController = placementCollider.GetComponent<BuildingPlacementController>();

            // Only show panels and outlines if building is not locked.
            if (!buildingPlacementController.isLocked())
            {
                // Show outline.
                outline = showOutline(placementCollider);

                // Check if mouse is clicked.
                if (Input.GetMouseButtonDown(0))
                {
                    // Get object data.
                    ObjectData objectData = buildingPlacementController.getObjectData();

                    // Check if object is bought or not.
                    if (objectData.isBought())
                    {
                        // Show defense upgrade panel.
                    }
                    else
                    {
                        // Show purchase panel.
                        createPurchasePanel(buildingPlacementController, objectData);
                    }
                }
            }
        }
        else
        {
            if (outline)
            {
                outline.enabled = false;
            }
        }
    }

    // Create purchase panel.
    void createPurchasePanel(BuildingPlacementController buildingPlacementController, ObjectData objectData)
    {
        // Close existing UI.
        closeExistingUI();

        // Load purchase panel with data.
        purchasePanelController.loadData(buildingPlacementController, objectData);
    }

    // Show outline on placement.
    Outline showOutline(Collider placementCollider)
    {
        // Get placement transform.
        Transform placementTransform = placementCollider.transform;

        // Get placement game object and building game object.
        GameObject placementGameObject = placementTransform.Find("Placement").gameObject;
        GameObject buildingGameObject = placementTransform.Find("Building").gameObject;

        // Show outlines.
        if (placementGameObject.activeSelf)
        {
            Outline outline = placementGameObject.GetComponent<Outline>();
            outline.enabled = true;

            return outline;
        }

        if (buildingGameObject.activeSelf)
        {
            Outline outline = buildingGameObject.GetComponent<Outline>();
            outline.enabled = true;

            return outline;
        }

        return null;
    }

    // Fade background in and load round scene.
    IEnumerator startRoundCoroutine()
    {
        yield return fadeBackgroundIn();

        SceneManager.LoadScene("Gavin's Round Scene");
    }

    // Fade background in.
    IEnumerator fadeBackgroundIn()
    {
        yield return StartCoroutine(loadingBackgroundController.fadeInCoroutine(.5f));
    }

    // Fade background out.
    IEnumerator fadeBackgroundOut()
    {
        yield return StartCoroutine(loadingBackgroundController.fadeOutCoroutine(.5f));
    }

    // Close purchase panel.
    void closePurchasePanel()
    {
        purchasePanelController.closePanel();
    }

    // Close popup panel.
    void closePopupPanel()
    {
        messagePopupPanelController.closePanel();
    }

    // Close existing UI.
    void closeExistingUI()
    {
        closePopupPanel();
        closePurchasePanel();
    }
}
