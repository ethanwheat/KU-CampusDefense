using UnityEngine;

public class AbilitiesButtonController : MonoBehaviour
{
    [SerializeField] private AbilitiesPanelController abilitiesPanel;
    
    public void OnAbilitiesButtonClicked()
    {
        abilitiesPanel.ShowAbilitiesPanel();
    }
}