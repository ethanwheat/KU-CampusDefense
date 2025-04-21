[System.Serializable]
public class LoanData
{
    public string LoanName;
    public int Amount;
    public int Debt;

    private GameData gameData;

    public LoanData(GameData gameData, int amount)
    {
        this.gameData = gameData;
        Amount = amount;
    }

    public void TakeLoan()
    {
        gameData.AddDollars(Amount);
        Debt += Amount;
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