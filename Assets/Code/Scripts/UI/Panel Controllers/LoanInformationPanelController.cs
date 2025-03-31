using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoanInformationPanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject lockedContent;
    [SerializeField] private TextMeshProUGUI lockedText;
    [SerializeField] private GameObject unlockedContent;
    [SerializeField] private Image unlockedLoanImage;
    [SerializeField] private GameObject takenContent;

    [SerializeField] private Image takenLoanImage;
    [SerializeField] private TextMeshProUGUI takenDebtCost;

    [Header("Unity Events")]
    public UnityEvent onTakeLoan;
    public UnityEvent onLoanPayment;

    private LoanData loanData;

    public void setData(LoanData data)
    {
        // Set loan data.
        loanData = data;

        // Set isLocked and debt.
        bool isLocked = loanData.isLocked();
        int debt = loanData.getDebt();

        // Set title text and cost text.
        titleText.text = loanData.getName();
        costText.text = loanData.getAmount().ToString();

        // Show locked content.
        if (isLocked)
        {
            lockedText.text = "Unlocks at round " + loanData.getUnlockRound() + ".";
            lockedContent.SetActive(true);
            return;
        }

        // Show taken content.
        if (debt > 0)
        {
            takenDebtCost.text = debt.ToString();
            takenLoanImage.sprite = loanData.getSprite();
            takenContent.SetActive(true);
            return;
        }

        // Show unlocked content.
        unlockedLoanImage.sprite = loanData.getSprite();
        unlockedContent.SetActive(true);
    }

    // Take loan and invoke onTakeLoan.
    public void onTake()
    {
        loanData.takeLoan();
        onTakeLoan.Invoke();
    }

    // Invoke onLoanPayment.
    public void onPayment()
    {
        onLoanPayment.Invoke();
    }
}
