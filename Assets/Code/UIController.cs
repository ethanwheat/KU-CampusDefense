using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject defensePanel;
    [SerializeField] private Button placeDefenseButton;
    [SerializeField] private Button closeDefensePanelButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeDefenseButton.onClick.AddListener(showDefensePanel);
        closeDefensePanelButton.onClick.AddListener(hideDefensePanel);
    }

    void showDefensePanel()
    {
        defensePanel.SetActive(true);
    }

    void hideDefensePanel()
    {
        defensePanel.SetActive(false);
    }
}
