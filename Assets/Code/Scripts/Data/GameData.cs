using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int roundNumber;
    public int dollars;
    public List<PurchasableData> purchasableData;
    public List<LoanData> loanData;
}