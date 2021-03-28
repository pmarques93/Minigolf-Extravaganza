using UnityEngine;

/// <summary>
/// Class responsible for handling animation events with black square.
/// </summary>
public class BlackSquareAnimationEvent : MonoBehaviour
{
    // Components
    private SceneController sceneController;
    private PlayerInputCustom input;

    private void Awake()
    {
        sceneController = FindObjectOfType<SceneController>();
        input = FindObjectOfType<PlayerInputCustom>();
    }

    public void RestartScene() =>
        sceneController.RestartCurrentScene();

    /// <summary>
    /// Loads a level on animation event.
    /// </summary>
    /// <param name="lvl">Level to load</param>
    public void LoadLevel(LevelEnum lvl) => sceneController.LoadLevel(lvl);
       
}
