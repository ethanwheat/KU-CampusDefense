[System.Serializable]
public class LoanData
{
    public string LoanName;
    public int Debt;

    [System.NonSerialized]
    private GameData gameData;

    public LoanData(GameData gameData, string loanName, int debt)
    {
        this.gameData = gameData;

        LoanName = loanName;
        Debt = debt;
    }

    public void PayDebt(int amount)
    {
        if (Debt - amount >= 0)
        {
            gameData.SubtractDollars(amount);
            Debt -= amount;
        }
    }

    public void SetDebt(int amount)
    {
        Debt = amount;
    }

    public void ResetLoan()
    {
        Debt = 0;
    }
}