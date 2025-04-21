using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObject", menuName = "Scriptable Objects/GameObject")]
public class GameDataObject : ScriptableObject
{
    [Header("Game Data")]
    [SerializeField] private int roundNumber;
    [SerializeField] private RoundDataObject selectedRound;
    [SerializeField] private int dollars;
    [SerializeField] private DefenseDataObject[] defenseDataObjects;
    [SerializeField] private AbilityDataObject[] abilityDataObjects;
    [SerializeField] private BonusDataObject[] bonusDataObjects;
    [SerializeField] private LoanDataObject[] loanDataObjects;

    public int RoundNumber => roundNumber;
    public RoundDataObject SelectedRound => selectedRound;
    public int Dollars => dollars;
    public DefenseDataObject[] DefenseDataObjects => defenseDataObjects;
    public BonusDataObject[] BonusDataObjects => bonusDataObjects;
    public LoanDataObject[] LoanDataObjects => loanDataObjects;
    public AbilityDataObject[] AbilityDataObjects => abilityDataObjects;

    // Increment round number.
    public void IncrementRoundNumber()
    {
        roundNumber += 1;
    }

    // Set selected round.
    public void SetSelectedRound(RoundDataObject currRound)
    {
        selectedRound = currRound;
    }

    // Add dollars.
    public void AddDollars(int amount)
    {
        dollars += amount;
    }

    // Subtract dollars.
    public void SubtractDollars(int amount)
    {
        int futureAmount = dollars - amount;

        if (futureAmount >= 0)
        {
            dollars = futureAmount;
        }
    }

    // Get debt.
    public int GetDebt()
    {
        int debt = 0;

        foreach (var data in loanDataObjects)
        {
            debt += data.Debt;
        }

        return debt;
    }

    // Pay debt.
    public void PayDebt(int amount)
    {
        int remaining = amount;

        foreach (var data in loanDataObjects)
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

    // Set game data.
    public void SetGameData(GameData gameData)
    {
        dollars = gameData.Dollars;
        roundNumber = gameData.RoundNumber;

        Dictionary<string, DefenseData> defenseDataDictionary = gameData.defenseData.ToDictionary(d => d.ObjectName);

        foreach (var defenseObject in defenseDataObjects)
        {
            string objectName = defenseObject.ObjectName;

            if (defenseDataDictionary.TryGetValue(objectName, out DefenseData matchingData))
            {
                defenseObject.SetBought(true);
                defenseObject.SetLevel(matchingData.Level);
            }
            else
            {
                defenseObject.SetBought(false);
                defenseObject.ResetLevel();
            }
        }

        Dictionary<string, BonusData> bonusDataDictionary = gameData.bonusData.ToDictionary(b => b.ObjectName);

        foreach (var bonusObject in bonusDataObjects)
        {
            string objectName = bonusObject.ObjectName;

            if (bonusDataDictionary.TryGetValue(objectName, out BonusData matchingData))
            {
                bonusObject.SetBought(true);
            }
            else
            {
                bonusObject.SetBought(false);
            }
        }

        Dictionary<string, LoanData> loanDataDictionary = gameData.loanData.ToDictionary(l => l.LoanName);

        foreach (var loanObject in loanDataObjects)
        {
            string objectName = loanObject.LoanName;

            if (loanDataDictionary.TryGetValue(objectName, out LoanData matchingData))
            {
                loanObject.SetDebt(matchingData.Debt);
            }
            else
            {
                loanObject.SetDebt(0);
            }
        }
    }
}
