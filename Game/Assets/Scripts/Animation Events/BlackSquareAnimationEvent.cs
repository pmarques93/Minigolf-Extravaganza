using UnityEngine;

/// <summary>
/// Class responsible for handling animation events with black square.
/// </summary>
public class BlackSquareAnimationEvent : MonoBehaviour
{
    public void RestartScene() => 
        FindObjectOfType<SceneController>().RestartCurrentScene();
}
