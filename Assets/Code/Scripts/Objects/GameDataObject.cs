using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObject", menuName = "Scriptable Objects/GameObject")]
public class GameDataObject : ScriptableObject
{
    [Header("Game Data")]
    [SerializeField] private RoundObject selectedRound;
    [SerializeField] private List<DefenseObject> defenseObjects;
    [SerializeField] private List<BonusObject> bonusObjects;
    [SerializeField] private List<LoanObject> loanObjects;
    [SerializeField] private List<AbilityObject> abilityObjects;

    public RoundObject SelectedRound => selectedRound;
    public List<DefenseObject> DefenseObjects => defenseObjects;
    public List<BonusObject> BonusObjects => bonusObjects;
    public List<LoanObject> LoanObjects => loanObjects;
    public List<AbilityObject> AbilityObjects => abilityObjects;
}
