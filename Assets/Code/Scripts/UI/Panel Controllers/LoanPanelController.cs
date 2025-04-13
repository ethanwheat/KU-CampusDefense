using TMPro;
using UnityEngine;

public class LoanPanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI debtCostText;
    [SerializeField] private Transform loanInformation;

    [Header("Panel Prefabs")]
    [SerializeField] private GameObject loanInformationPanelPrefab;

    [Header("UI Controllers")]
    [SerializeField] private BuildingSceneUIController buildingSceneUIController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Sounds")]
    [SerializeField] private AudioClip errorSoundEffect;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    // Load purchase panel data.
    public void showPanel()
    {
        // Update loan information panels.
        updatePanel();

        // Show panel
        gameObject.SetActive(true);
    }

    void updatePanel()
    {
        // Set debt text.
        debtCostText.text = gameDataController.getDebt().ToString();

        // Destroy previous loan information panels.
        foreach (Transform loanInformationPanel in loanInformation)
        {
            Destroy(loanInformationPanel.gameObject);
        }

        // Get loan data.
        LoanData[] loanData = gameDataController.getLoanData();

        // Create loan information panels.
        for (int i = 0; i < loanData.Length; i++)
        {
            // Create loan information panel, set position, set data, add onTakeLoan listener, onLoanPayment listener.
            LoanData data = loanData[i];
            GameObject loanInformationPanel = Instantiate(loanInformationPanelPrefab, loanInformation);
            LoanInformationPanelController loanInformationPanelController = loanInformationPanel.GetComponent<LoanInformationPanelController>();
            loanInformationPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -80 * i - 37.5f, 0);
            loanInformationPanelController.setData(data);
            loanInformationPanelController.onTakeLoan.AddListener(refreshUI);
            loanInformationPanelController.onLoanPayment.AddListener(() => onPayment(data));
        }
    }

    // Update dollar UI and update panel.
    void refreshUI()
    {
        buildingSceneUIController.updateDollarUI();
        updatePanel();
    }

    // Make payment
    void onPayment(LoanData loanData)
    {
        // Get loan name, dollars, and debt.
        string loanName = loanData.getName();
        int dollars = gameDataController.getDollarAmount();
        int debt = loanData.getDebt();

        // Check if user has money.
        if (dollars > 0)
        {
            // Pay what user can.
            loanData.payDebt(Mathf.Clamp(dollars, 0, debt));

            // Refresh UI.
            refreshUI();

            return;
        }

        // Show error popup panel if user has no money and close panel.
        SoundManager.instance.playSoundEffect(errorSoundEffect, transform, 1f);
        messagePopupPanelController.showPanel("Insufficient Dollars", "You do not have enough dollars to make a payment on " + loanName + "!");
        closePanel();
    }

    // Close purchase panel.
    public void closePanel()
    {
        gameObject.SetActive(false);
    }
}
