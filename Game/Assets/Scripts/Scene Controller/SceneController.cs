using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Class responsible for handling scene management.
/// </summary>
public class SceneController : MonoBehaviour
{
    public Scene CurrentScene() => SceneManager.GetActiveScene();

    /// <summary>
    /// Loads current scene.
    /// </summary>
    public void RestartCurrentScene() => 
        StartCoroutine(LoadNewScene(CurrentScene()));

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
    private IEnumerator LoadNewScene(Scene scene)
    {
        YieldInstruction waitForFrame = new WaitForEndOfFrame();
        AsyncOperation sceneToLoad =
            SceneManager.LoadSceneAsync(scene.name);

        // After the progress reaches 1, the scene loads
        while (sceneToLoad.progress < 1)
        {
            yield return waitForFrame;
        }

        yield return null;
    }

    /// <summary>
    /// Coroutine that loads a new scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns null.</returns>
    private IEnumerator LoadNewScene(LevelEnum level)
    {
        YieldInstruction waitForFrame = new WaitForEndOfFrame();
        AsyncOperation sceneToLoad =
            SceneManager.LoadSceneAsync(level.ToString());

        // After the progress reaches 1, the scene loads
        while (sceneToLoad.progress < 1)
        {
            yield return waitForFrame;
        }

        yield return null;
    }
}
