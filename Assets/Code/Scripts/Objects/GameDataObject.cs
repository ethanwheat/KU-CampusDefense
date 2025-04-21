using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObject", menuName = "Scriptable Objects/GameObject")]
public class GameDataObject : ScriptableObject
{
    [Header("Game Data")]
    [SerializeField] private RoundObject selectedRound;

    public RoundObject SelectedRound => selectedRound;
}
