using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoundSceneUIController : MonoBehaviour
{
    [Header("UI Controllers")]
    [SerializeField] private DefensePanelController defensePanelController;
    [SerializeField] private RegenHealthPanelController regenHealthPanelController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI dollarText;
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("Round Manager")]
    [SerializeField] private RoundManager roundManager;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private Camera mainCamera;
    private Outline healthBuildingOutline;

    void Start()
    {
        // Set main camera.
        mainCamera = Camera.main;

        // Update dollar UI and coin UI.
        updateDollarUI();
        updateCoinUI();

        // Fade background out.
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

            // Get hit collider.
            Collider collider = hit.collider;

            // If hit collider tag is ObjectBuilding then call handleObjectBuilding.
            if (collider.CompareTag("HealthBuilding"))
            {
                handleHealthBuilding(hit);
                return;
            }

            return;
        }

        // If no raycat hit and there is a healthBuildingOutline then hide outline.
        if (healthBuildingOutline)
        {
            healthBuildingOutline.enabled = false;
        }
    }

    void handleHealthBuilding(RaycastHit hit)
    {
        // Set loanBuildingOutline and enable outline.
        healthBuildingOutline = hit.collider.GetComponent<Outline>();
        healthBuildingOutline.enabled = true;

        // Check if mouse is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            // Close existing UI and show loan panel.
            closeExistingUI();
            regenHealthPanelController.showPanel();
        }
    }

    // Update dollar UI.
    public void updateDollarUI()
    {
        // Update dollar text.
        dollarText.text = gameDataController.getDollarAmount().ToString();
    }

    // Update coin UI.
    public void updateCoinUI()
    {
        // Update coin text.
        coinText.text = roundManager.getCoinAmount().ToString();
    }

    // Show defense panel.
    public void showDefensePanel()
    {
        bool showPanel = !defensePanelController.gameObject.activeSelf;

        closeExistingUI();

        if (showPanel)
        {
            defensePanelController.showPanel();
        }
    }

    // Close existing UI.
    void closeExistingUI()
    {
        defensePanelController.closePanel();
        regenHealthPanelController.closePanel();
        messagePopupPanelController.closePanel();
    }
}
