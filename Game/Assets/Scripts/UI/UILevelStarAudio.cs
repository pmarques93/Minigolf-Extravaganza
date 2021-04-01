using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// /Class responsible for playing star's pop up sounds in animation events.
/// </summary>
public class UILevelStarAudio : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFirstStar()
    {
        if (audioSource != null)
        {
            audioSource.pitch = 0.5f;
            audioSource.Play();
        }
    }

    public void PlaySoundSecondStar()
    {
        if (audioSource != null)
        {
            audioSource.pitch = 0.75f;
            audioSource.Play();
        }
    }

    public void PlaySoundThirdStar()
    {
        if (audioSource != null)
        {
            audioSource.pitch = 1f;
            audioSource.Play();
        }
    }
}
