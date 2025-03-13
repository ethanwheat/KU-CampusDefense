using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefensePlacementButtonController : MonoBehaviour
{
    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI defenseCostText;

    [Header("UI Images")]
    [SerializeField] private Image defenseImage;

    [Header("Sprites")]
    [SerializeField] private Sprite unselectedPressedSprite;
    [SerializeField] private Sprite unselectedUnpressedSprite;
    [SerializeField] private Sprite selectedPressedSprite;
    [SerializeField] private Sprite selectedUnpressedSprite;

    public void setData(DefenseData defenseData)
    {
        Sprite sprite = defenseData.getSprite();

        if (sprite)
        {
            defenseImage.sprite = sprite;
        }

        defenseText.text = defenseData.getName();
        defenseCostText.text = defenseData.getCoinCost().ToString();
    }

    public void onSelect()
    {
        // Change current button to be selected.
        Image buttonImage = gameObject.GetComponent<Image>();
        buttonImage.sprite = selectedUnpressedSprite;

        // Reset unpressed button sprite.
        Button selectedButton = gameObject.GetComponent<Button>();
        SpriteState spriteState;
        spriteState.pressedSprite = selectedPressedSprite;
        selectedButton.spriteState = spriteState;
    }

    public void onDeselect()
    {
        // Reset pressed button sprite.
        Image selectedButtonImage = gameObject.GetComponent<Image>();
        selectedButtonImage.sprite = unselectedUnpressedSprite;

        // Reset unpressed button sprite.
        Button selectedButton = gameObject.GetComponent<Button>();
        SpriteState spriteState;
        spriteState.pressedSprite = unselectedPressedSprite;
        selectedButton.spriteState = spriteState;
    }
}
