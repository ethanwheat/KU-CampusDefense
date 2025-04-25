using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip clickSoundEffect;

    // Play sound effect when button clicked.
    public void OnClick()
    {
        SoundManager.instance.PlaySoundEffect(clickSoundEffect, transform, volume: 1f);
    }
}
