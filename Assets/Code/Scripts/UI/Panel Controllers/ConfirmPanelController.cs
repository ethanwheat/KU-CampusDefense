using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmPanelController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("UI Controllers")]
    [SerializeField] private PanelFadeController panelFadeController;

    [Header("Untiy Events")]
    public UnityEvent OnConfirm;
    public UnityEvent OnQuit;

    private bool isFading;

    // Show panel with correct content.
    public void ShowPanel(string title, string description, bool fade)
    {
        isFading = fade;

        titleText.text = title;
        descriptionText.text = description;

        if (isFading)
        {
            panelFadeController.Show();
            return;
        }

        gameObject.SetActive(true);
    }

    // Hide panel.
    void HidePanel()
    {
        if (isFading)
        {
            panelFadeController.Hide();
            return;
        }

        gameObject.SetActive(false);
    }

    // Called when confirm is clicked.
    public void OnConfirmClick()
    {
        HidePanel();
        OnConfirm.Invoke();
        OnConfirm.RemoveAllListeners();
    }

    // Called when panel is closed.
    public void OnQuitClick()
    {
        HidePanel();
        OnQuit.Invoke();
        OnQuit.RemoveAllListeners();
    }
}
