using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoanPanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI debtText;
    [SerializeField] private Transform loanInformation;

    [Header("Panel Prefabs")]
    [SerializeField] private GameObject loanInformationPanelPrefab;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    // Load purchase panel data.
    public void showPanel()
    {
        // Update loan information panels.
        updateLoanInformationPanels();

        // Show panel
        gameObject.SetActive(true);
    }

    void updateLoanInformationPanels()
    {
        // Destroy previous loan information panels.
        foreach (Transform transform in loanInformation)
        {
            Destroy(transform);
        }

        // Get loan data.
        LoanData[] loanData = gameDataController.getLoanData();

        // Create loan information panels.
        foreach (var data in loanData)
        {
            GameObject loanInformationPanel = Instantiate(loanInformationPanelPrefab, loanInformation);
        }
    }

    // Close purchase panel.
    public void closePanel()
    {
        gameObject.SetActive(false);
    }
}
