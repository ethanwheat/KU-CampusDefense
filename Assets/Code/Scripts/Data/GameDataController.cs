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

    public int RoundNumber => roundNumber;
    public RoundData SelectedRound => selectedRound;
    public int Dollars => dollars;
    public DefenseData[] DefenseData => defenseData;
    public BonusData[] BonusData => bonusData;
    public LoanData[] LoanData => loanData;
    public AbilityData[] AbilityData => abilityData;

    // Increment round number.
    public void incrementRoundNumber()
    {
        roundNumber += 1;
    }

    public void setSelectedRound(RoundData currRound)
    {
        selectedRound = currRound;
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
