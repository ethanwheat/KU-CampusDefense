using System.Collections.Generic;
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

    [Header("Game Data Object")]
    [SerializeField] private GameDataObject gameDataObject;

    private GameData gameData;

    void Start()
    {
        gameData = GameDataManager.instance.GameData;
    }

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
        debtCostText.text = gameData.GetDebt().ToString();

        // Destroy previous loan information panels.
        foreach (Transform loanInformationPanel in loanInformation)
        {
            Destroy(loanInformationPanel.gameObject);
        }

        // Create loan information panels.
        foreach (var loanObject in gameDataObject.LoanObjects)
        {
            // Create loan information panel, set position, set data, add onTakeLoan listener, onLoanPayment listener.
            LoanData loanData = gameData.GetLoanData(loanObject.LoanName);
            GameObject loanInformationPanel = Instantiate(loanInformationPanelPrefab, loanInformation);
            LoanInformationPanelController loanInformationPanelController = loanInformationPanel.GetComponent<LoanInformationPanelController>();

            loanInformationPanelController.SetData(loanObject, loanData);
            loanInformationPanelController.OnTakeLoan.AddListener(RefreshUI);
            loanInformationPanelController.OnLoanPayment.AddListener(() => OnPayment(loanObject, loanData));
        }
    }

    // Update dollar UI and update panel.
    void RefreshUI()
    {
        buildingSceneUIController.UpdateDollarUI();
        UpdatePanel();
    }

    // Make payment
    void OnPayment(LoanObject loanObject, LoanData loanData)
    {
        // Get loan name, dollars, and debt.
        string loanName = loanObject.LoanName;
        int dollars = gameData.Dollars;
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
