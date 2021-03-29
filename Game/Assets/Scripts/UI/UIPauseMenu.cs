using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject initialSelectedGameobject;
    [SerializeField] private Animator blackSquare;

    // Menu variables
    /// <summary>
    /// This getter is used to know if the player is on default pause menu screen
    /// or in another menu.
    /// </summary>
    public bool OnMainPauseMenu { get; private set; }

    // Components
    private EventSystem eventSys;

    /// <summary>
    /// Getter to control the selection of resume button.
    /// </summary>
    public Coroutine ObjectEnabledCoroutine { get; set; }

    private void Awake()
    {
        eventSys = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        if (ObjectEnabledCoroutine == null)
            ObjectEnabledCoroutine = StartCoroutine(ObjectEnabled());
    }

    /// <summary>
    /// After end of frame, sets current object selected to null.
    /// </summary>
    /// <returns>WaitForEndOfFrame.</returns>
    private IEnumerator ObjectEnabled()
    {
        yield return new WaitForEndOfFrame();
        OnMainPauseMenu = true;
        eventSys.SetSelectedGameObject(null);
    }

    private void Update()
    {
        // Every time there is not an object selected, it selects resume button
        if (eventSys.currentSelectedGameObject == null)
        {
            eventSys.SetSelectedGameObject(initialSelectedGameobject);
        }
    }

    /// <summary>
    /// Controls if the player is in main pause menu.
    /// </summary>
    public void OnMainPauseMenuToFalse() => OnMainPauseMenu = false;
    /// <summary>
    /// Controls if the player is in main pause menu.
    /// </summary>
    public void OnMainPauseMenuToTrue() => OnMainPauseMenu = true;

    public void ResumeGame()
    {
        FindObjectOfType<PauseMenu>().PauseBehaviour(PauseEnum.Unpause);
    }

    /// <summary>
    /// Restarts current level.
    /// </summary>
    public void RestartLevel()
    {
        blackSquare.SetTrigger("Restart");
        FindObjectOfType<PauseMenu>().UnpauseGame();
        FindObjectOfType<PlayerInputCustom>().SwitchControlsToPauseMenu();
    }

    public void GoToMainMenu()
    {
        // Current level being passed to select it on main menu
        PlayerPrefs.SetString(
            "LastLevelPassed", FindObjectOfType<LevelPassed>().CurrentLevel.ToString());

        blackSquare.SetTrigger("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
