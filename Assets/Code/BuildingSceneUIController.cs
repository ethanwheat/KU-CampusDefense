using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BuildingSceneUIController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject purchasePanelPrefab;
    [SerializeField] private GameObject messagePopupPanelPrefab;
    [SerializeField] private GameObject backgroundPrefab;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private GameObject purchasePanel;
    private GameObject messagePopupPanel;
    private GameObject dollarUI;
    private GameObject background;
    private Camera mainCamera;
    private Outline outline;

    void Start()
    {
        // Set main camera.
        mainCamera = Camera.main;

        // Set dollar UI and dollar UI text.
        dollarUI = transform.Find("DollarUI").gameObject;
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
        // Close existing popup panel.
        closePopupPanel();

        // Create message popup panel.
        messagePopupPanel = Instantiate(messagePopupPanelPrefab, transform);
        messagePopupPanel.GetComponent<MessagePopupPanelController>().setData(messageTitle, messageText);
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

        // Create purchase panel.
        purchasePanel = Instantiate(purchasePanelPrefab, transform);

        // Load purchase panel with data.
        PurchasePanelController purchasePanelController = purchasePanel.GetComponent<PurchasePanelController>();
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
        if (background)
        {
            Destroy(background);
        }

        background = Instantiate(backgroundPrefab, transform);
        BackgroundController backgroundController = background.GetComponent<BackgroundController>();

        yield return StartCoroutine(backgroundController.fadeInCoroutine(.5f));
    }

    // Fade background out.
    IEnumerator fadeBackgroundOut()
    {
        background = Instantiate(backgroundPrefab, transform);
        BackgroundController backgroundController = background.GetComponent<BackgroundController>();

        yield return StartCoroutine(backgroundController.fadeOutCoroutine(.5f));

        Destroy(background);

    }

    // Close purchase panel.
    void closePurchasePanel()
    {
        // Close purchase panel if it exists.
        if (purchasePanel)
        {
            PurchasePanelController controller = purchasePanel.GetComponent<PurchasePanelController>();
            controller.closePanel();
        }
    }

    // Close popup panel.
    void closePopupPanel()
    {
        // Close popup panel if it exists.
        if (messagePopupPanel)
        {
            MessagePopupPanelController messagePopupPanelController = messagePopupPanel.GetComponent<MessagePopupPanelController>();
            messagePopupPanelController.closePanel();
        }
    }

    // Close existing UI.
    void closeExistingUI()
    {
        closePopupPanel();
        closePurchasePanel();
    }
}
