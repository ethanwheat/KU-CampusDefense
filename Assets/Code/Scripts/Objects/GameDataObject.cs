using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataObject", menuName = "Scriptable Objects/GameDataObject")]
public class GameDataObject : ScriptableObject
{
    [Header("Game Data Objects")]
    [SerializeField] private List<DefenseObject> defenseObjects;
    [SerializeField] private List<BonusObject> bonusObjects;
    [SerializeField] private List<LoanObject> loanObjects;
    [SerializeField] private List<AbilityObject> abilityObjects;
    [SerializeField] private List<RoundObject> roundObjects;

    public List<DefenseObject> DefenseObjects => defenseObjects;
    public List<BonusObject> BonusObjects => bonusObjects;
    public List<LoanObject> LoanObjects => loanObjects;
    public List<AbilityObject> AbilityObjects => abilityObjects;
    public List<RoundObject> RoundObjects => roundObjects;

    // Get round object from number.
    public RoundObject GetRoundObject(int roundNumber)
    {
        return roundObjects[roundNumber - 1];
    }
}
