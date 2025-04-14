using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip clickSoundEffect;

    // Play sound effect when button clicked.
    public void onClick()
    {
        SoundManager.instance.playSoundEffect(clickSoundEffect, transform, 1f);
    }
}
