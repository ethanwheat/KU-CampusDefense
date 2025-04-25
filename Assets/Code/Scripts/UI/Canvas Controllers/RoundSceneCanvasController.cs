using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoundSceneCanvasController : MonoBehaviour
{
    public static RoundSceneCanvasController instance;

    [Header("UI Controllers")]
    [SerializeField] private DefensePanelController defensePanelController;
    [SerializeField] private AbilitiesPanelController abilitiesPanelController;
    [SerializeField] private RegenHealthPanelController regenHealthPanelController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;
    [SerializeField] private RoundWonPanelController roundWonPanelController;
    [SerializeField] private RoundLostPanelController roundLostPanelController;
    [SerializeField] private SmallMessagePopupPanelController smallMessagePopupPanelController;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI dollarText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Sounds")]
    [SerializeField] private AudioClip clickSoundEffect;

    private Camera mainCamera;
    private Outline healthBuildingOutline;
    private RoundManager roundManager;

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

        // Set round manager.
        roundManager = RoundManager.instance;

        // Update dollar UI and coin UI.
        UpdateDollarUI();
        UpdateCoinUI();

        // Fade background out.
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

            // If hit collider tag is ObjectBuilding then call handleObjectBuilding.
            if (collider.CompareTag("HealthBuilding"))
            {
                HandleHealthBuilding(hit);
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

    void HandleHealthBuilding(RaycastHit hit)
    {
        // Set loanBuildingOutline and enable outline.
        healthBuildingOutline = hit.collider.GetComponent<Outline>();
        healthBuildingOutline.enabled = true;

        // Check if mouse is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            // Play click sound effect.
            SoundManager.instance.PlaySoundEffect(clickSoundEffect, transform, volume: 1f);

            // Close existing UI and show loan panel.
            CloseExistingUI();
            regenHealthPanelController.ShowPanel();
        }
    }

    // Update dollar UI.
    public void UpdateDollarUI()
    {
        // Update dollar text.
        dollarText.text = GameDataManager.instance.GameData.Dollars.ToString();
    }

    // Update coin UI.
    public void UpdateCoinUI()
    {
        // Update coin text.
        coinText.text = roundManager.Coins.ToString();
    }

    public void UpdateWaveUI(int currWave, int numWaves)
    {
        waveText.text = "Wave " + currWave.ToString() + "/" + numWaves.ToString();
    }

    // Show defense panel.
    public void ShowDefensePanel()
    {
        if (defensePanelController == null)
        {
            return;
        }
        bool showPanel = !defensePanelController.gameObject.activeSelf;

        CloseExistingUI();

        if (showPanel)
        {
            defensePanelController.ShowPanel();
        }
    }

    public void ShowAbilitiesPanel()
    {
        if (abilitiesPanelController == null)
        {
            return;
        }

        bool showPanel = !abilitiesPanelController.gameObject.activeSelf;
        CloseExistingUI();

        if (showPanel)
        {
            abilitiesPanelController.ShowPanel();
        }
    }

    // Close existing UI (This is buggy with the abilities panel)
    public void ShowWavePopupPanel(string waveNum)
    {
        smallMessagePopupPanelController.ShowPanel("Wave " + waveNum);
    }

    public void ShowRoundWonPanel(string round, string reward, string loan, string total)
    {
        string[] avoid = { "RoundWonPanel" };
        HideAllUI(avoid);
        roundWonPanelController.ShowPanel(round, reward, loan, total);
    }

    public void ShowRoundLostPanel()
    {
        string[] avoid = { "RoundLostPanel" };
        HideAllUI(avoid);
        roundLostPanelController.ShowPanel();
    }

    // Close existing UI.
    public void CloseExistingUI()
    {
        defensePanelController.ClosePanel();
        regenHealthPanelController.ClosePanel();
        messagePopupPanelController.ClosePanel();
        abilitiesPanelController.ClosePanel();
    }

    void HideAllUI(string[] avoid)
    {
        CloseExistingUI();

        foreach (Transform transform in transform)
        {
            GameObject uiElement = transform.gameObject;

            if (!avoid.Contains(uiElement.name) && uiElement.activeSelf)
            {
                uiElement.SetActive(false);
            }
        }
    }
}
