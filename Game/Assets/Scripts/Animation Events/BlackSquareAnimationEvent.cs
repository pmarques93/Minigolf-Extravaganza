using UnityEngine;

/// <summary>
/// Class responsible for handling animation events with black square.
/// </summary>
public class BlackSquareAnimationEvent : MonoBehaviour
{
    // Components
    private SceneController sceneController;
    private BallHandler ball;
    private PlayerInputCustom input;

    private void Awake()
    {
        sceneController = FindObjectOfType<SceneController>();
        ball = FindObjectOfType<BallHandler>();
        input = FindObjectOfType<PlayerInputCustom>();
    }

    private void OnEnable()
    {
        if (ball != null) ball.Victory += TriggerAnimationToMainMenu;
    }

    private void OnDisable()
    {
        if (ball != null) ball.Victory -= TriggerAnimationToMainMenu;
    }

    private void Update()
    {
        if (ball == null)
        {
            ball = FindObjectOfType<BallHandler>();
            if (ball != null) ball.Victory += TriggerAnimationToMainMenu;
        }
    }

    /// <summary>
    /// Disables all controls.
    /// </summary>
    public void DisableControls() => input.SwitchControlsToDisableOnMenu();

    /// <summary>
    /// Loads main menu after victory.
    /// </summary>
    private void TriggerAnimationToMainMenu() =>
        GetComponent<Animator>().SetTrigger("MainMenu");

    public void RestartScene() =>
        sceneController.RestartCurrentScene();

    /// <summary>
    /// Loads a level on animation event.
    /// </summary>
    /// <param name="lvl">Level to load</param>
    public void LoadLevel(LevelEnum lvl) => sceneController.LoadLevel(lvl);
       
}
