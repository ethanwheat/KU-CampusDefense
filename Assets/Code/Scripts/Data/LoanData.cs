using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LoanData", menuName = "Scriptable Objects/LoanData")]
public class LoanData : ScriptableObject
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
    [SerializeField] private GameDataController gameDataController;

    public string getName()
    {
        return loanName;
    }

    public int getAmount()
    {
        return amount;
    }

    public bool isUnlocked()
    {
        return unlockRound > gameDataController.getRoundNumber();
    }

    public void takeLoan()
    {
        debt += amount;
    }

    public int getDebt()
    {
        return debt;
    }

    public void payDebt(int amount)
    {
        if (debt - amount >= 0)
        {
            debt -= amount;
        }
    }

    public void resetLoan()
    {
        debt = 0;
    }
}
