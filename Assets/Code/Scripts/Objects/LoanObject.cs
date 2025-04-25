using UnityEngine;

[CreateAssetMenu(fileName = "LoanDataObject", menuName = "Scriptable Objects/LoanDataObject")]
public class LoanObject : ScriptableObject
{
    [Header("Loan Information")]
    [SerializeField] private string loanName;
    [SerializeField] private int amount;
    [SerializeField] private int unlockRound;

    [Header("Sprites")]
    [SerializeField] private Sprite sprite;

    public string LoanName => loanName;
    public int Amount => amount;
    public int UnlockRound => unlockRound;
    public Sprite Sprite => sprite;
}
