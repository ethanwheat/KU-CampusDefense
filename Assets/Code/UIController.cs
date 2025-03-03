using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button startRoundButton;
    [SerializeField] private Button placeDefenseButton;
    [SerializeField] private GameObject defensePanelPrefab;
    [SerializeField] private GameObject roundManagerGameObject;

    private GameObject defensePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startRoundButton.onClick.AddListener(startRound);
        placeDefenseButton.onClick.AddListener(showDefensePanel);
    }

    void startRound()
    {
        RoundManager roundManager = roundManagerGameObject.GetComponent<RoundManager>();
        roundManager.StartRound();
    }

    void showDefensePanel()
    {
        if (!defensePanel)
        {
            defensePanel = Instantiate(defensePanelPrefab, transform);
        }
    }
}
