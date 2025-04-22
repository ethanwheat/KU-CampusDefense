using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataObject", menuName = "Scriptable Objects/GameDataObject")]
public class GameDataObject : ScriptableObject
{
    [Header("Game Objects")]
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
}
