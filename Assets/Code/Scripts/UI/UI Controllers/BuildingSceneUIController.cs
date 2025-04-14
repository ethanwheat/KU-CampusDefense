using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BuildingSceneUIController : MonoBehaviour
{
    [Header("UI Controllers")]
    [SerializeField] private PurchasePanelController purchasePanelController;
    [SerializeField] private UpgradePanelController upgradePanelController;
    [SerializeField] private LoanPanelController loanPanelController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI dollarText;

    [Header("Sounds")]
    [SerializeField] private AudioClip clickSoundEffect;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private Camera mainCamera;
    private BuildingPlacementController buildingPlacementController;
    private Outline loanBuildingOutline;

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

            // Get hit collider.
            Collider collider = hit.collider;

            // If hit collider tag is DefenseBuilding or BonusBuilding then call handleDefenseBonusBuilding.
            if (collider.CompareTag("DefenseBuilding") || collider.CompareTag("BonusBuilding"))
            {
                handleDefenseBonusBuilding(hit);
                return;
            }

            // If hit collider tag is LoanBuilding then call handleLoanBuilding.
            if (collider.CompareTag("LoanBuilding"))
            {
                handleLoanBuilding(hit);
                return;
            }

            return;
        }

        // If no raycat hit and there is a buildingPlacementController then hide outline.
        if (buildingPlacementController)
        {
            buildingPlacementController.showOutline(false);
        }

        // If no raycat hit and there is a loanBuildingOutline then hide outline.
        if (loanBuildingOutline)
        {
            loanBuildingOutline.enabled = false;
        }
    }

    void handleDefenseBonusBuilding(RaycastHit hit)
    {
        // Get building placement controller, object data, isLocked, isBought, and isDefenseBuilding.
        buildingPlacementController = hit.collider.GetComponent<BuildingPlacementController>();
        PurchasableData purchasableData = buildingPlacementController.getPurchasableData();
        bool isLocked = purchasableData.isLocked();
        bool isBought = purchasableData.isBought();
        bool isDefenseBuilding = hit.collider.CompareTag("DefenseBuilding");

        // Make sure the object is not locked.
        if (isLocked)
        {
            return;
        }

        // Show outline.
        if (!isBought || isDefenseBuilding)
        {
            buildingPlacementController.showOutline(true);
        }

        // Check if mouse is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            // Close existing UI.
            closeExistingUI();

            // Play click sound effect.
            SoundManager.instance.playSoundEffect(clickSoundEffect, transform, 1f);

            // Show purchase panel with data if not bought.
            if (!purchasableData.isBought())
            {
                purchasePanelController.showPanel(buildingPlacementController, purchasableData);
                return;
            }

            // Show upgrade panel if object type is defense.
            if (isDefenseBuilding)
            {
                upgradePanelController.showPanel(buildingPlacementController.getBuildingName(), (DefenseData)purchasableData);
                return;
            }
        }
    }

    void handleLoanBuilding(RaycastHit hit)
    {
        // Set loanBuildingOutline and enable outline.
        loanBuildingOutline = hit.collider.GetComponent<Outline>();
        loanBuildingOutline.enabled = true;

        // Check if mouse is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            // Play click sound effect.
            SoundManager.instance.playSoundEffect(clickSoundEffect, transform, 1f);

            // Close existing UI and show loan panel.
            closeExistingUI();
            loanPanelController.showPanel();
        }
    }

    // Start round.
    public void startRound()
    {
        SoundManager.instance.stopMusic(.5f);
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
        dollarText.text = gameDataController.Dollars.ToString();
    }

    // Close existing UI.
    void closeExistingUI()
    {
        purchasePanelController.closePanel();
        upgradePanelController.closePanel();
        loanPanelController.closePanel();
        messagePopupPanelController.closePanel();
    }
}
