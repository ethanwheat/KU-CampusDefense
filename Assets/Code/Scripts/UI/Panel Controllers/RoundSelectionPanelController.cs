using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundSelectionPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform contentParent; // Content under the ScrollView
    [SerializeField] private GameObject roundButtonPrefab; // Prefab for each round button

    [Header("Game Object")]
    [SerializeField] private GameDataObject gameDataObject;

    private void Start()
    {
        PopulateScrollView();
    }

    private void PopulateScrollView()
    {
        GameDataManager gameDataMangaer = GameDataManager.instance;
        GameData gameData = gameDataMangaer.GameData;
        List<RoundObject> roundObjects = gameDataObject.RoundObjects;

        int maxUnlocked = gameData.RoundNumber;

        foreach (var round in roundObjects)
        {
            bool isUnlocked = round.RoundNumber <= maxUnlocked;
            if (isUnlocked)
            {
                GameObject btnObj = Instantiate(roundButtonPrefab, contentParent);

                // set button label
                btnObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Round " + round.RoundNumber;

                btnObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    gameDataMangaer.SetSelectedRound(round);
                    ClosePanel();
                });
            }
        }
    }

    // Show message popup panel and set message popup panel data.
    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    // Close message popup panel.
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
