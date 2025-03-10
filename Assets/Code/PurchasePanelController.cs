using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PurchasePanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI costText;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    private ObjectData objectData;
    private string objectName;
    private string objectDescription;
    private int objectCost;
    private string buildingName;
    private GameObject errorMessagePopupPanel;

    public void Start()
    {

    }

    public void loadData(string name, ObjectData data)
    {
        // Set building name.
        buildingName = name;

        // Set object data.
        objectData = data;
        objectName = objectData.getName();
        objectDescription = objectData.getDescription();
        objectCost = objectData.getDollarCost();

        // Set UI text.
        headerText.text = buildingName;
        itemText.text = objectName;
        itemDescription.text = objectDescription;
        costText.text = objectCost.ToString();
    }

    public void onPurchase()
    {
        int dollars = gameDataController.getDollars();

        if (dollars >= objectCost)
        {

        }
        else
        {
            createErrorPopupPanel("Insufficient Dollars", "You do not have enough dollars to buy " + buildingName + "!");
            closePanel();
        }
    }

    void createErrorPopupPanel(string messageTitle, string messageText)
    {
        transform.parent.GetComponent<BuildingSceneUIController>().createMessagePopupPanel(messageTitle, messageText);
    }

    public void closePanel()
    {
        Destroy(gameObject);
    }
}
