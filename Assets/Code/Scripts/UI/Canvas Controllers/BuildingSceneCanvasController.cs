using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BuildingSceneUIController : MonoBehaviour
{
    public static BuildingSceneUIController instance;

    [Header("UI Controllers")]
    [SerializeField] private PurchasePanelController purchasePanelController;
    [SerializeField] private UpgradePanelController upgradePanelController;
    [SerializeField] private LoanPanelController loanPanelController;
    [SerializeField] private RoundSelectionPanelController roundSelectionPanelController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI dollarText;
    [SerializeField] private TextMeshProUGUI selectedText;

    [Header("Sounds")]
    [SerializeField] private AudioClip clickSoundEffect;

    private Camera mainCamera;
    private BuildingPlacementController buildingPlacementController;
    private Outline loanBuildingOutline;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Set main camera.
        mainCamera = Camera.main;

        // Update dollar UI.
        UpdateDollarUI();
        // Update selected round
        UpdateSelectedRound();

        // Fade out background.
        StartCoroutine(loadingBackgroundController.FadeOutCoroutine(.5f));
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
                HandleDefenseBonusBuilding(hit);
                return;
            }

            // If hit collider tag is LoanBuilding then call handleLoanBuilding.
            if (collider.CompareTag("LoanBuilding"))
            {
                HandleLoanBuilding(hit);
                return;
            }

            return;
        }

        // If no raycat hit and there is a buildingPlacementController then hide outline.
        if (buildingPlacementController)
        {
            buildingPlacementController.ShowOutline(false);
        }

        // If no raycat hit and there is a loanBuildingOutline then hide outline.
        if (loanBuildingOutline)
        {
            loanBuildingOutline.enabled = false;
        }
    }

    void HandleDefenseBonusBuilding(RaycastHit hit)
    {
        // Get building placement controller, game data, purchasable object, purchasable data, and isDefenseBuilding.
        buildingPlacementController = hit.collider.GetComponent<BuildingPlacementController>();
        GameData gameData = GameDataManager.instance.GameData;
        PurchasableObject purchasableObject = buildingPlacementController.PurchasableObject;
        PurchasableData purchasableData = gameData.GetPurchasableData(purchasableObject.ObjectName);
        bool isDefenseBuilding = hit.collider.CompareTag("DefenseBuilding");

        // Make sure the object is not locked.
        if (gameData.RoundNumber < purchasableObject.UnlockRound || (purchasableData != null && !isDefenseBuilding))
        {
            return;
        }

        // Show outline.
        buildingPlacementController.ShowOutline(true);

        // Check if mouse is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            // Close existing UI.
            CloseExistingUI();

            // Play click sound effect.
            SoundManager.instance.PlaySoundEffect(clickSoundEffect, transform, volume: 1f);

            // Show purchase panel with data if not bought.
            if (purchasableData == null)
            {
                purchasePanelController.ShowPanel(buildingPlacementController, purchasableObject);
                return;
            }

            // Show upgrade panel if object type is defense.
            if (isDefenseBuilding)
            {
                upgradePanelController.ShowPanel(buildingPlacementController.BuildingName, (DefenseObject)purchasableObject, (DefenseData)purchasableData);
                return;
            }
        }
    }

    void HandleLoanBuilding(RaycastHit hit)
    {
        // Set loanBuildingOutline and enable outline.
        loanBuildingOutline = hit.collider.GetComponent<Outline>();
        loanBuildingOutline.enabled = true;

        // Check if mouse is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            // Play click sound effect.
            SoundManager.instance.PlaySoundEffect(clickSoundEffect, transform, volume: 1f);

            // Close existing UI and show loan panel.
            CloseExistingUI();
            loanPanelController.ShowPanel();
        }
    }

    // Start round.
    public void StartRound()
    {
        SoundManager.instance.StopMusic();
        StartCoroutine(StartRoundCoroutine());
    }

    // Fade background in and load round scene.
    IEnumerator StartRoundCoroutine()
    {
        PauseMenuCanvasController.instance.enabled = false;

        yield return StartCoroutine(loadingBackgroundController.FadeInCoroutine(.5f));

        SceneManager.LoadScene("Round Scene");
    }

    // Update the dollar UI.
    public void UpdateDollarUI()
    {
        // Update dollar text.
        dollarText.text = GameDataManager.instance.GameData.Dollars.ToString();
    }

    // Show defense panel.
    public void ShowRoundSelectionPanel()
    {
        bool showPanel = !roundSelectionPanelController.gameObject.activeSelf;

        CloseExistingUI();

        if (showPanel)
        {
            roundSelectionPanelController.ShowPanel();
        }
    }

    public void UpdateSelectedRound()
    {
        int roundNumber = GameDataManager.instance.SelectedRound.RoundNumber;
        selectedText.text = "Round " + roundNumber.ToString();
    }

    // Close existing UI.
    public void CloseExistingUI()
    {
        purchasePanelController.ClosePanel();
        upgradePanelController.ClosePanel();
        loanPanelController.ClosePanel();
        roundSelectionPanelController.ClosePanel();
        messagePopupPanelController.ClosePanel();
    }
}
