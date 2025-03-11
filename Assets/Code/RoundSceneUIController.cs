using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundSceneUIController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject defensePanelPrefab;
    [SerializeField] private GameObject messagePopupPanelPrefab;
    [SerializeField] private GameObject backgroundPrefab;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private GameObject defensePanel;
    private GameObject messagePopupPanel;
    private GameObject dollarUI;
    private GameObject coinUI;
    private GameObject background;
    private RoundManager roundManager;

    void Start()
    {
        // Set dollar UI and coin UI.
        dollarUI = transform.Find("DollarUI").gameObject;
        coinUI = transform.Find("CoinUI").gameObject;

        // Set round manager.
        foreach (GameObject currentGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (currentGameObject.name == "RoundManager")
            {
                roundManager = currentGameObject.GetComponent<RoundManager>();
                break;
            }
        }

        // Update dollar UI and coin UI.
        updateDollarUI();
        updateCoinUI();

        // Fade background out.
        StartCoroutine(fadeBackgroundOut());
    }

    // Create defense panel.
    public void createDefensePanel()
    {
        // Close existing UI.
        closeDefensePanel();

        // Create defense panel.
        defensePanel = Instantiate(defensePanelPrefab, transform);
    }

    // Update dollar UI.
    public void updateDollarUI()
    {
        // Update dollar text.
        TextMeshProUGUI dollarText = dollarUI.transform.Find("DollarText").GetComponent<TextMeshProUGUI>();
        dollarText.text = gameDataController.getDollarAmount().ToString();
    }

    // Update coin UI.
    public void updateCoinUI()
    {
        // Update dollar text.
        TextMeshProUGUI coinText = coinUI.transform.Find("CoinText").GetComponent<TextMeshProUGUI>();
        coinText.text = roundManager.getCoinAmount().ToString();
    }

    // Create message popup panel.
    public void createMessagePopupPanel(string messageTitle, string messageText)
    {
        // Close existing popup panel.
        closePopupPanel();

        // Create message popup panel.
        messagePopupPanel = Instantiate(messagePopupPanelPrefab, transform);
        messagePopupPanel.GetComponent<MessagePopupPanelController>().setData(messageTitle, messageText);
    }

    // Fade background in slowly.
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

    // Fade background out slowly.
    IEnumerator fadeBackgroundOut()
    {
        background = Instantiate(backgroundPrefab, transform);
        BackgroundController backgroundController = background.GetComponent<BackgroundController>();

        yield return StartCoroutine(backgroundController.fadeOutCoroutine(.5f));

        Destroy(background);
    }

    // Close defense panel.
    void closeDefensePanel()
    {
        // Close defemse panel if it exists.
        if (defensePanel)
        {
            DefensePanelController defensePanelController = defensePanel.GetComponent<DefensePanelController>();
            defensePanelController.closePanel();
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
        // Close all existing UI.
        closePopupPanel();
        closeDefensePanel();
    }
}
