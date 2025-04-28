using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int RoundNumber;
    public int Dollars;
    public List<DefenseData> DefenseData = new List<DefenseData>();
    public List<BonusData> BonusData = new List<BonusData>();
    public List<LoanData> LoanData = new List<LoanData>();

    // Increment round number.
    public void IncrementRoundNumber()
    {
        RoundNumber += 1;
        TriggerAutosave();
    }

    // Add dollars.
    public void AddDollars(int amount)
    {
        Dollars += amount;
        TriggerAutosave();
    }

    // Subtract dollars.
    public void SubtractDollars(int amount)
    {
        Dollars = Mathf.Max(0, Dollars - amount);
        TriggerAutosave();
    }

    public void CreateDefenseData(DefenseObject defenseObject)
    {
        DefenseData defenseData = new DefenseData(defenseObject.ObjectName);
        DefenseData.Add(defenseData);
        TriggerAutosave();
    }

    public void CreateBonusData(BonusObject bonusObject)
    {
        BonusData bonusData = new BonusData(bonusObject.ObjectName);
        BonusData.Add(bonusData);
        TriggerAutosave();
    }

    public PurchasableData GetPurchasableData(string name)
    {
        List<PurchasableData> purchasableData = new List<PurchasableData>();
        purchasableData.AddRange(DefenseData);
        purchasableData.AddRange(BonusData);

        return purchasableData.Find(data => data.ObjectName == name);
    }

    public DefenseData GetDefenseData(string name)
    {
        return DefenseData.Find(data => data.ObjectName == name);
    }

    public BonusData GetBonusData(string name)
    {
        return BonusData.Find(data => data.ObjectName == name);
    }

    public LoanData GetLoanData(string name)
    {
        return LoanData.Find(data => data.LoanName == name);
    }

    public void TakeLoan(LoanObject loanObject)
    {
        string name = loanObject.LoanName;
        int amount = loanObject.Amount;

        LoanData loanData = new LoanData(name, amount);

        AddDollars(amount);
        LoanData.Add(loanData);
        TriggerAutosave();
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

    // Pay debt on loan.
    public void PayDebtOnLoan(LoanData loanData, int amount)
    {
        loanData.SubtractDebt(amount);
        SubtractDollars(amount);

        if (loanData.Debt == 0)
        {
            LoanData.Remove(loanData);
        }

        TriggerAutosave();

    }

    // Pay debt on all loans.
    public void PayDebtOnAllLoans(int amount)
    {
        int remaining = amount;

        foreach (var data in LoanData)
        {
            int debt = data.Debt;

            if (remaining > 0 && debt > 0)
            {
                int payment = Mathf.Min(remaining, debt);
                data.SetDebt(debt - payment);

                if (debt == 0)
                {
                    LoanData.Remove(data);
                }

                remaining -= payment;
            }
        }

        TriggerAutosave();
    }

    public void TriggerAutosave()
    {
        GameDataManager.instance.TriggerAutosave();
    }
}