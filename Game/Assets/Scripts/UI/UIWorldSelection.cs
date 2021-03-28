using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Class responsible for handling world selection UI.
/// </summary>
public class UIWorldSelection : MonoBehaviour
{
    public void PlayVideo(VideoPlayer video)
    {
        video.Play();
    }

    public void StopVideo(VideoPlayer video)
    {
        video.Stop();
    }
}
