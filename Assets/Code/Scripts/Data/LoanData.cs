using UnityEngine;

[System.Serializable]
public class LoanData
{
    public string LoanName;
    public int Debt;

    public LoanData(string loanName, int debt)
    {
        LoanName = loanName;
        Debt = debt;
    }

    public void SetDebt(int amount)
    {
        Debt = Mathf.Max(0, amount);
    }

    public void SubtractDebt(int amount)
    {
        Debt = Mathf.Max(0, Debt - amount);
    }
}