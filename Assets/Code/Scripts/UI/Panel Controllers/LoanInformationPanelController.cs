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
    public UnityEvent OnTakeLoan;
    public UnityEvent OnLoanPayment;

    private LoanObject loanObject;

    public void SetData(LoanObject loanObject, LoanData loanData)
    {
        // Set loan data.
        this.loanObject = loanObject;

        // Set title text and cost text.
        titleText.text = loanObject.LoanName;
        costText.text = loanObject.Amount.ToString();

        // Set unlock round.
        int unlockRound = loanObject.UnlockRound;

        // Show locked content.
        if (GameDataManager.instance.GameData.RoundNumber < unlockRound)
        {
            lockedText.text = "Unlocks at round " + unlockRound + ".";
            lockedContent.SetActive(true);
            return;
        }

        // Set debt.
        int debt = loanData != null ? loanData.Debt : 0;

        // Show taken content.
        if (debt > 0)
        {
            takenDebtCost.text = debt.ToString();
            takenLoanImage.sprite = loanObject.Sprite;
            takenContent.SetActive(true);
            return;
        }

        // Show unlocked content.
        unlockedLoanImage.sprite = loanObject.Sprite;
        unlockedContent.SetActive(true);
    }

    // Play take loan sound effect, take loan and invoke onTakeLoan.
    public void OnTake()
    {
        SoundManager.instance.PlaySoundEffect(takeLoanSoundEffect, transform, 1f);
        GameDataManager.instance.GameData.TakeLoan(loanObject);
        OnTakeLoan.Invoke();
    }

    // Play make payment sound effect, invoke onLoanPayment.
    public void OnPayment()
    {
        SoundManager.instance.PlaySoundEffect(makePaymentSoundEffect, transform, 1f);
        OnLoanPayment.Invoke();
    }
}
