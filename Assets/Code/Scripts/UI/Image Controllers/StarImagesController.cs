using UnityEngine;
using UnityEngine.UI;

public class StarImagesController : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;

    [Header("Sprites")]
    [SerializeField] private Sprite unfilledStarSprite;
    [SerializeField] private Sprite filledStarSprite;

    public void UpdateStars(int level)
    {
        if (level >= 1)
        {
            star1.sprite = filledStarSprite;
        }
        else
        {
            star1.sprite = unfilledStarSprite;
        }

        if (level >= 2)
        {
            star2.sprite = filledStarSprite;
        }
        else
        {
            star2.sprite = unfilledStarSprite;
        }

        if (level == 3)
        {
            star3.sprite = filledStarSprite;
        }
        else
        {
            star3.sprite = unfilledStarSprite;
        }
    }
}
