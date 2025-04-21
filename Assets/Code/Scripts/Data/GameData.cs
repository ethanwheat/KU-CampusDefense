using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int RoundNumber;
    public int Dollars;
    public List<DefenseData> DefenseData;
    public List<BonusData> BonusData;
    public List<LoanData> LoanData;

    public GameData(
        int roundNumber = 1,
        int dollars = 100,
        List<DefenseData> defenseData = null,
        List<BonusData> bonusData = null,
        List<LoanData> loanData = null
    )
    {
        RoundNumber = roundNumber;
        Dollars = dollars;
        DefenseData = defenseData ?? new List<DefenseData> { new DefenseData("Barrier", true) };
        BonusData = bonusData ?? new List<BonusData>();
        LoanData = loanData ?? new List<LoanData>();
    }

    // Increment round number.
    public void IncrementRoundNumber()
    {
        RoundNumber += 1;
    }

    // Add dollars.
    public void AddDollars(int amount)
    {
        Dollars += amount;
    }

    // Subtract dollars.
    public void SubtractDollars(int amount)
    {
        int futureAmount = Dollars - amount;

        if (futureAmount >= 0)
        {
            Dollars = futureAmount;
        }
    }

    public DefenseData GetDefenseData(string name)
    {
        foreach (DefenseData data in DefenseData)
        {
            if (data.ObjectName == name)
            {
                return data;
            }
        }

        return null;
    }

    // Get debt.
    public int GetDebt()
    {
        int debt = 0;

        foreach (var data in LoanData)
        {
            debt += data.Debt;
        }

        return debt;
    }

    // Pay debt.
    public void PayDebt(int amount)
    {
        int remaining = amount;

        foreach (var data in LoanData)
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
}