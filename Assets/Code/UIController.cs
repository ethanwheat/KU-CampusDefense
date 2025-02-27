using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button placeDefenseButton;
    [SerializeField] private GameObject defensePanelPrefab;

    private GameObject defensePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeDefenseButton.onClick.AddListener(showDefensePanel);
    }

    void showDefensePanel()
    {
        if (!defensePanel)
        {
            defensePanel = Instantiate(defensePanelPrefab, transform);
        }
    }
}
