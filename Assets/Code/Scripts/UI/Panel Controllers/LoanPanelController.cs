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
    public void ShowPanel()
    {
        // Update loan information panels.
        UpdatePanel();

        // Show panel
        gameObject.SetActive(true);
    }

    void UpdatePanel()
    {
        // Set debt text.
        debtCostText.text = gameDataController.GetDebt().ToString();

        // Destroy previous loan information panels.
        foreach (Transform loanInformationPanel in loanInformation)
        {
            Destroy(loanInformationPanel.gameObject);
        }

        // Get loan data.
        LoanData[] loanData = gameDataController.LoanData;

        // Create loan information panels.
        for (int i = 0; i < loanData.Length; i++)
        {
            // Create loan information panel, set position, set data, add onTakeLoan listener, onLoanPayment listener.
            LoanData data = loanData[i];
            GameObject loanInformationPanel = Instantiate(loanInformationPanelPrefab, loanInformation);
            LoanInformationPanelController loanInformationPanelController = loanInformationPanel.GetComponent<LoanInformationPanelController>();
            loanInformationPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -80 * i - 37.5f, 0);
            loanInformationPanelController.SetData(data);
            loanInformationPanelController.onTakeLoan.AddListener(RefreshUI);
            loanInformationPanelController.onLoanPayment.AddListener(() => OnPayment(data));
        }
    }

    // Update dollar UI and update panel.
    void RefreshUI()
    {
        buildingSceneUIController.updateDollarUI();
        UpdatePanel();
    }

    // Make payment
    void OnPayment(LoanData loanData)
    {
        // Get loan name, dollars, and debt.
        string loanName = loanData.LoanName;
        int dollars = gameDataController.Dollars;
        int debt = loanData.Debt;

        // Check if user has money.
        if (dollars > 0)
        {
            // Pay what user can.
            loanData.PayDebt(Mathf.Clamp(dollars, 0, debt));

            // Refresh UI.
            RefreshUI();

            return;
        }

        // Show error popup panel if user has no money and close panel.
        SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, 1f);
        messagePopupPanelController.ShowPanel("Insufficient Dollars", "You do not have enough dollars to make a payment on " + loanName + "!");
        ClosePanel();
    }

    // Close purchase panel.
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
