using UnityEngine;

/// <summary>
/// Class responsible for handling animation events with black square.
/// </summary>
public class BlackSquareAnimationEvent : MonoBehaviour
{
    // Components
    private SceneController sceneController;
    private PlayerInputCustom input;
    private Animator anim;
    private HighscoreHandler highScoreHandler;

    private void Awake()
    {
        sceneController = FindObjectOfType<SceneController>();
        input = FindObjectOfType<PlayerInputCustom>();
        anim = GetComponent<Animator>();
        highScoreHandler = FindObjectOfType<HighscoreHandler>();
    }

    private void OnEnable()
    {
        if (highScoreHandler != null)
        {
            highScoreHandler.BeatHighscore += () => anim.SetTrigger("BeatHighScore");
            highScoreHandler.NoBeatHighscore += () => anim.SetTrigger("NoBeatHighScore");
        }
    }

    private void OnDisable()
    {
        if (highScoreHandler != null)
        {
            highScoreHandler.BeatHighscore -= () => anim.SetTrigger("BeatHighScore");
            highScoreHandler.NoBeatHighscore -= () => anim.SetTrigger("NoBeatHighScore");
        }
    }

    /// <summary>
    /// Disables all controls.
    /// </summary>
    public void DisableControls() => input.SwitchControlsToDisableOnMenu();

    /// <summary>
    /// Loads main menu after victory.
    /// Triggered by UIPauseMenu.
    /// </summary>
    public void MainMenu() => anim.SetTrigger("MainMenu");

    public void RestartScene() =>
        sceneController.RestartCurrentScene();

    /// <summary>
    /// Loads a level on animation event.
    /// </summary>
    /// <param name="lvl">Level to load</param>
    public void LoadLevel(LevelEnum lvl) => sceneController.LoadLevel(lvl);}
