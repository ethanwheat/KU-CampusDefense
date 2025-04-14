using TMPro;
using UnityEngine;

public class RoundWonPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private TextMeshProUGUI loanText;
    [SerializeField] private TextMeshProUGUI totalText;

    // Show message popup panel and set message popup panel data.
    public void showPanel(string round, string reward, string loan, string total)
    {
        roundText.text = "Congrats, you have completed Round " + round ;
        rewardText.text = "$" + reward;
        loanText.text = loan;
        totalText.text = total;

        gameObject.SetActive(true);
    }

    // Close message popup panel.
    public void closePanel()
    {
        gameObject.SetActive(false);
    }
}
