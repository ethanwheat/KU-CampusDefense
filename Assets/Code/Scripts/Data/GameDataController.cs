using UnityEngine;

[CreateAssetMenu(fileName = "GameDataController", menuName = "Scriptable Objects/GameDataController")]
public class GameDataController : ScriptableObject
{
    [Header("Game Initialization")]
    [SerializeField] private int startingDollarAmount;

    [Header("Game Data")]
    [SerializeField] private int roundNumber;
    [SerializeField] private RoundData selectedRound;
    [SerializeField] private int dollars;
    [SerializeField] private DefenseData[] defenseData;
    [SerializeField] private AbilityData[] abilityData;
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

    public RoundData getSelectedRound()
    {
        return selectedRound;
    }

    public void setSelectedRound(RoundData currRound)
    {
        selectedRound = currRound;
    }

    // Get dollar amount.
    public int getDollarAmount()
    {
        return dollars;
    }

    // Add dollars.
    public void addDollars(int amount)
    {
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

    // Get ability data
    public AbilityData[] getAbilityData()
    {
        return abilityData;
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
  
    public void payDebt(int amount)
    {
        int remaining = amount;

        foreach (var data in loanData)
        {
            int debt = data.getDebt();

            if (remaining > 0 && debt > 0)
            {
                int payment = Mathf.Min(remaining, debt);    
                data.setDebt(debt - payment);
                remaining -= payment;
            }
        }
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
