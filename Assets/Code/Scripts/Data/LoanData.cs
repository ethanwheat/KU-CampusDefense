[System.Serializable]

public class LoanData
{
    public string loanName;
    public int debt;

    public string LoanName => loanName;
    public int Debt => debt;
}