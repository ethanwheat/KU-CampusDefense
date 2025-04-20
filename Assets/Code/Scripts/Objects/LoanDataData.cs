using UnityEngine;

[CreateAssetMenu(fileName = "LoanDataObject", menuName = "Scriptable Objects/LoanDataObject")]
public class LoanDataObject : ScriptableObject
{
    [Header("Loan Information")]
    [SerializeField] private string loanName;
    [SerializeField] private int amount;
    [SerializeField] private int unlockRound;

    [Header("Sprites")]
    [SerializeField] private Sprite sprite;

    [Header("Loan Data")]
    [SerializeField] private int debt;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataObject gameDataController;

    public string LoanName => loanName;
    public int Amount => amount;
    public int UnlockRound => unlockRound;
    public bool Locked => unlockRound > gameDataController.RoundNumber;
    public Sprite Sprite => sprite;
    public int Debt => debt;

    public void TakeLoan()
    {
        gameDataController.AddDollars(amount);
        debt += amount;
    }

    public void PayDebt(int amount)
    {
        if (debt - amount >= 0)
        {
            gameDataController.SubtractDollars(amount);
            debt -= amount;
        }
    }

    public void SetDebt(int amount)
    {
        debt = amount;
    }

    public void ResetLoan()
    {
        debt = 0;
    }
}
