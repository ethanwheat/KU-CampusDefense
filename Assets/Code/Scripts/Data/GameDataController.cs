using UnityEngine;

[CreateAssetMenu(fileName = "GameDataController", menuName = "Scriptable Objects/GameDataController")]
public class GameDataController : ScriptableObject
{
    [Header("Game Initialization")]
    [SerializeField] private int startingDollarAmount;

    [Header("Game Data")]
    [SerializeField] private int roundNumber;
    [SerializeField] private int dollars;
    [SerializeField] private DefenseData[] defenseData;
    [SerializeField] private BonusData[] bonusData;

    // Get round number.
    public int getRoundNumber()
    {
        return roundNumber;
    }

    // Increment round number.
    public void incrementRoundNumber()
    {
        roundNumber += 1;
    }

    // Get dollar amount.
    public int getDollarAmount()
    {
        return dollars;
    }

    // Add dollars.
    public void addDollars(int amount)
    {
        dollars += amount;
    }

    // Subtract dollars.
    public void subtractDollars(int amount)
    {
        int futureAmount = dollars - amount;

        if (futureAmount >= 0)
        {
            dollars = futureAmount;
        }
    }

    // Get defense data.
    public DefenseData[] getDefenseData()
    {
        return defenseData;
    }

    // Get bonus data.
    public BonusData[] getBonusData()
    {
        return bonusData;
    }

    // Reset game data.
    public void resetGameData()
    {
        // Reset dollars to starting dollar amount.
        dollars = startingDollarAmount;

        foreach (var data in defenseData)
        {
            bool isBoughtAtStart = data.isBoughtAtStart();
            data.setBought(isBoughtAtStart);
        }

        foreach (var data in bonusData)
        {
            bool isBoughtAtStart = data.isBoughtAtStart();
            data.setBought(isBoughtAtStart);
        }
    }
}
