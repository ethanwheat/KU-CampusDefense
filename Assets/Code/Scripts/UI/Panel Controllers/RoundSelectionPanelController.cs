using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundSelectionPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform contentParent; // Content under the ScrollView
    [SerializeField] private GameObject roundButtonPrefab; // Prefab for each round button
    [SerializeField] private GameObject lockedRoundPrefab; // Prefab for locked round buttons
    [SerializeField] private BuildingSceneUIController uiController;

    [Header("Game Object")]
    [SerializeField] private GameDataObject gameDataObject;

    private void Start()
    {
        PopulateScrollView();
    }

    private void PopulateScrollView()
    {
        GameDataManager gameDataManager = GameDataManager.instance;
        GameData gameData = gameDataManager.GameData;
        List<RoundObject> roundObjects = gameDataObject.RoundObjects;

        int maxUnlocked = gameData.RoundNumber;

        foreach (var round in roundObjects)
        {
            bool isUnlocked = round.RoundNumber <= maxUnlocked;

            GameObject btnObj;

            if (isUnlocked)
            {
                btnObj = Instantiate(roundButtonPrefab, contentParent);

                // set button label
                btnObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Round " + round.RoundNumber;

                btnObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    gameDataManager.SetSelectedRound(round);
                    ClosePanel();
                });
            } 
            else
            {
                btnObj = Instantiate(lockedRoundPrefab, contentParent);
                // set button label
                btnObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Round " + round.RoundNumber;
            }
        }
    }

    // Show message popup panel and set message popup panel data.
    public void ShowPanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // Close message popup panel.
    public void ClosePanel()
    {
        gameObject.SetActive(false);
        uiController.UpdateSelectedRound();
    }
}
