using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int roundNumber;
    public int dollars;
    public List<DefenseData> defenseData;
    public List<BonusData> bonusData;
    public List<LoanData> loanData;

    public int RoundNumber => roundNumber;
    public int Dollars => dollars;
    public List<DefenseData> DefenseData => defenseData;
    public List<BonusData> BonusData => bonusData;
    public List<LoanData> LoanData => loanData;

    public GameData(
        int roundNumber = 1,
        int dollars = 100,
        List<DefenseData> defenseData = null,
        List<BonusData> bonusData = null,
        List<LoanData> loanData = null
    )
    {
        this.roundNumber = roundNumber;
        this.dollars = dollars;
        this.defenseData = defenseData ?? new List<DefenseData>
        {
            new DefenseData("Barrier", 1)
        };
        this.bonusData = bonusData ?? new List<BonusData>();
        this.loanData = loanData ?? new List<LoanData>();
    }
}