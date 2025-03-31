using UnityEngine;

[CreateAssetMenu(fileName = "GameDataController", menuName = "Scriptable Objects/GameDataController")]
public class GameDataController : ScriptableObject
{
    [Header("Game Initialization")]
    [SerializeField] private int startingDollarAmount;

    [Header("Game Data")]
    [SerializeField] private int roundNumber;
    [SerializeField] private int dollars;
    [SerializeField] private DefenseData[] defenseData;
    [SerializeField] private BonusData[] bonusData;
    [SerializeField] private LoanData[] loanData;

    // Get round number.
    public int getRoundNumber()
    {
        return roundNumber;
    }

    // Increment round number.
    public void incrementRoundNumber()
    {
        roundNumber += 1;
    }

    // Get dollar amount.
    public int getDollarAmount()
    {
        return dollars;
    }

    // Add dollars.
    public void addDollars(int amount)
    {
      Debug.Log("In addollars");
        dollars += amount;
    }

    // Subtract dollars.
    public void subtractDollars(int amount)
    {
        int futureAmount = dollars - amount;

        if (futureAmount >= 0)
        {
            dollars = futureAmount;
        }
    }

    // Get defense data.
    public DefenseData[] getDefenseData()
    {
        return defenseData;
    }

    // Get bonus data.
    public BonusData[] getBonusData()
    {
        return bonusData;
    }

    // Get loan data.
    public LoanData[] getLoanData()
    {
        return loanData;
    }

    // Get debt.
    public int getDebt()
    {
        int debt = 0;

        foreach (var data in loanData)
        {
            debt += data.getDebt();
        }

        return debt;
    }

    // Reset game data.
    public void resetGameData()
    {
        // Reset dollars to starting dollar amount and round number to 1.
        dollars = startingDollarAmount;
        roundNumber = 1;

        // Reset defense data.
        foreach (var data in defenseData)
        {
            bool isBoughtAtStart = data.isBoughtAtStart();
            data.setBought(isBoughtAtStart);
            data.resetLevel();
        }

        // Reset bonus data.
        foreach (var data in bonusData)
        {
            bool isBoughtAtStart = data.isBoughtAtStart();
            data.setBought(isBoughtAtStart);
        }

        // Reset loan data.
        foreach (var data in loanData)
        {
            data.resetLoan();
        }
    }
}
