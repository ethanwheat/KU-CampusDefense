using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundSceneUIController : MonoBehaviour
{
    [Header("UI Controllers")]
    [SerializeField] private DefensePanelController defensePanelController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI dollarText;
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("Round Manager")]
    [SerializeField] private RoundManager roundManager;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    void Start()
    {
        // Update dollar UI and coin UI.
        updateDollarUI();
        updateCoinUI();

        // Fade background out.
        StartCoroutine(loadingBackgroundController.fadeOutCoroutine(.5f));
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
        // Update dollar text.
        coinText.text = roundManager.getCoinAmount().ToString();
    }
}
