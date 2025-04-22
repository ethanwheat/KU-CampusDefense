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
        Debt = amount;
    }

    public void SubtractDebt(int amount)
    {
        if (Debt - amount >= 0)
        {
            Debt -= amount;
        }
    }
}