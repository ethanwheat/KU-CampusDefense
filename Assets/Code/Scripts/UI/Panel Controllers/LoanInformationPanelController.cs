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

    [Header("Sounds")]
    [SerializeField] private AudioClip takeLoanSoundEffect;
    [SerializeField] private AudioClip makePaymentSoundEffect;

    [Header("Unity Events")]
    public UnityEvent onTakeLoan;
    public UnityEvent onLoanPayment;

    private LoanData loanData;

    public void SetData(LoanData data)
    {
        // Set loan data.
        loanData = data;

        // Set isLocked and debt.
        bool isLocked = loanData.Locked;
        int debt = loanData.Debt;

        // Set title text and cost text.
        titleText.text = loanData.LoanName;
        costText.text = loanData.Amount.ToString();

        // Show locked content.
        if (isLocked)
        {
            lockedText.text = "Unlocks at round " + loanData.UnlockRound + ".";
            lockedContent.SetActive(true);
            return;
        }

        // Show taken content.
        if (debt > 0)
        {
            takenDebtCost.text = debt.ToString();
            takenLoanImage.sprite = loanData.Sprite;
            takenContent.SetActive(true);
            return;
        }

        // Show unlocked content.
        unlockedLoanImage.sprite = loanData.Sprite;
        unlockedContent.SetActive(true);
    }

    // Play take loan sound effect, take loan and invoke onTakeLoan.
    public void OnTake()
    {
        SoundManager.instance.PlaySoundEffect(takeLoanSoundEffect, transform, 1f);
        loanData.TakeLoan();
        onTakeLoan.Invoke();
    }

    // Play make payment sound effect, invoke onLoanPayment.
    public void OnPayment()
    {
        SoundManager.instance.PlaySoundEffect(makePaymentSoundEffect, transform, 1f);
        onLoanPayment.Invoke();
    }
}
