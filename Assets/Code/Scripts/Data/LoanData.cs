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

    public int getUnlockRound()
    {
        return unlockRound;
    }

    public bool isLocked()
    {
        return unlockRound > gameDataController.RoundNumber;
    }

    public Sprite getSprite()
    {
        return sprite;
    }

    public void takeLoan()
    {
        gameDataController.addDollars(amount);
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
            gameDataController.subtractDollars(amount);
            debt -= amount;
        }
    }

    public void setDebt(int amount)
    {
        debt = amount;
    }

    public void resetLoan()
    {
        debt = 0;
    }
}
