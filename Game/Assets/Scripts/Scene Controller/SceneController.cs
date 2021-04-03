using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handling scene management.
/// </summary>
public class SceneController : MonoBehaviour
{
    // Components
    private Animator anim;
    private HighscoreHandler highScoreHandler;
    private LevelPassed levelPassed;

    [SerializeField] private Image loadingBar;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        highScoreHandler = FindObjectOfType<HighscoreHandler>();
        levelPassed = GetComponent<LevelPassed>();
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
    /// Gets current scene.
    /// </summary>
    /// <returns></returns>
    public Scene CurrentScene() => SceneManager.GetActiveScene();

    /// <summary>
    /// Loads current scene.
    /// </summary>
    public void RestartCurrentScene() => 
        StartCoroutine(LoadNewScene(levelPassed.CurrentLevel));

    /// <summary>
    /// Loads a level.
    /// </summary>
    /// <param name="level">Level to load.</param>
    public void LoadLevel(LevelEnum level) =>
        StartCoroutine(LoadNewScene(level));

    /// <summary>
    /// Coroutine that loads a new scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns null.</returns>
    private IEnumerator LoadNewScene(LevelEnum level)
    {
        YieldInstruction WaitForEndOfFrame = new WaitForEndOfFrame();

        yield return WaitForEndOfFrame;

        DisableControls();
        // Triggers transition to area animation
        anim.SetTrigger("LevelTransition");

        yield return WaitForEndOfFrame;
        while (anim.GetCurrentAnimatorStateInfo(0).IsTag("LevelTransition"))
        {
            yield return WaitForEndOfFrame;
        }
        // After the animation is over

        // Triggers transition to area animation
        anim.SetTrigger("Loading");

        // Asyc loads a scene
        AsyncOperation sceneToLoad =
            SceneManager.LoadSceneAsync(level.ToString());

        // After the progress of the async operation reaches 1, the scene loads
        while (sceneToLoad.progress < 1)
        {
            loadingBar.fillAmount = sceneToLoad.progress;
            yield return null;
        }
        yield return null;
    }

    /// <summary>
    /// Disables all controls.
    /// Called on black square animation event.
    /// </summary>
    public void DisableControls()
    {
        PlayerInputCustom input = FindObjectOfType<PlayerInputCustom>();
        if (input != null) input.SwitchControlsToDisableOnMenu();
    }
}
