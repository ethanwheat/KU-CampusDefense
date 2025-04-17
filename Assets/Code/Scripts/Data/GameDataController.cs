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
    public void IncrementRoundNumber()
    {
        roundNumber += 1;
    }

    // Set selected round.
    public void SetSelectedRound(RoundData currRound)
    {
        selectedRound = currRound;
    }

    // Add dollars.
    public void AddDollars(int amount)
    {
        dollars += amount;
    }

    // Subtract dollars.
    public void SubtractDollars(int amount)
    {
        int futureAmount = dollars - amount;

        if (futureAmount >= 0)
        {
            dollars = futureAmount;
        }
    }

    // Get debt.
    public int GetDebt()
    {
        int debt = 0;

        foreach (var data in loanData)
        {
            debt += data.Debt;
        }

        return debt;
    }

    // Pay debt.
    public void PayDebt(int amount)
    {
        int remaining = amount;

        foreach (var data in loanData)
        {
            int debt = data.Debt;

            if (remaining > 0 && debt > 0)
            {
                int payment = Mathf.Min(remaining, debt);
                data.SetDebt(debt - payment);
                remaining -= payment;
            }
        }
    }

    // Reset game data.
    public void ResetGameData()
    {
        // Reset dollars to starting dollar amount and round number to 1.
        dollars = startingDollarAmount;
        roundNumber = 1;

        // Reset defense data.
        foreach (var data in defenseData)
        {
            bool isBoughtAtStart = data.BoughtAtStart;
            data.SetBought(isBoughtAtStart);
            data.ResetLevel();
        }

        // Reset bonus data.
        foreach (var data in bonusData)
        {
            bool isBoughtAtStart = data.BoughtAtStart;
            data.SetBought(isBoughtAtStart);
        }

        // Reset loan data.
        foreach (var data in loanData)
        {
            data.ResetLoan();
        }
    }
}
