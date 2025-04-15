using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundSelectionPanelController : MonoBehaviour
{
    [Header("Round Configuration")]
    [SerializeField] private RoundData[] allRounds;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    [Header("UI References")]
    [SerializeField] private Transform contentParent; // Content under the ScrollView
    [SerializeField] private GameObject roundButtonPrefab; // Prefab for each round button

    private void Start()
    {
        PopulateScrollView();
    }

    private void PopulateScrollView()
    {
        int maxUnlocked = gameDataController.RoundNumber;

        foreach (var round in allRounds)
        {
            bool isUnlocked = round.roundNumber <= maxUnlocked;
            if (isUnlocked)
            {
                GameObject btnObj = Instantiate(roundButtonPrefab, contentParent);

                // set button label
                btnObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Round " + round.roundNumber;

                RoundData capturedRound = round; // closure-safe

                btnObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    gameDataController.setSelectedRound(capturedRound);
                    closePanel();
                });
            }
        }
    }

    // Show message popup panel and set message popup panel data.
    public void showPanel()
    {
        gameObject.SetActive(true);
    }

    // Close message popup panel.
    public void closePanel()
    {
        gameObject.SetActive(false);
    }
}
