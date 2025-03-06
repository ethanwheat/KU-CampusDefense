using TMPro;
using UnityEngine;

public class PurchasePanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI costText;

    public void loadData(string buildingName, ObjectData objectData)
    {
        // Get object data.
        string objectName = objectData.getName();
        string objectDescription = objectData.getDescription();
        string objectCost = objectData.getDiamondCost().ToString();

        // Set UI text.
        headerText.text = buildingName;
        itemText.text = objectName;
        itemDescription.text = objectDescription;
        costText.text = objectCost;
    }

    public void closePanel()
    {
        Destroy(gameObject);
    }
}
