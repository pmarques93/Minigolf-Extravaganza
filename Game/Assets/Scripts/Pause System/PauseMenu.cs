using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling 
/// </summary>
public class PauseMenu : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;
    private BallShot ballShot;
    [SerializeField] private UIPauseMenu uiPauseMenu;

    // Pause variables
    public bool GamePaused { get; private set; }

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        ballShot = FindObjectOfType<BallShot>();
    }

    private void Start()
    {
        UnpauseGame();
    }

    private void OnEnable()
    {
        input.PauseGame += PauseBehaviour;
    }

    private void OnDisable()
    {
        input.PauseGame -= PauseBehaviour;
    }

    private void Update()
    {
        if (ballShot == null) ballShot = FindObjectOfType<BallShot>();
    }

    /// <summary>
    /// Handles pause behaviour. Pauses or unpauses the game.
    /// </summary>
    /// <param name="typeOfPause"></param>
    public void PauseBehaviour(PauseEnum typeOfPause)
    {
        switch(typeOfPause)
        {
            case PauseEnum.Pause:

                // Only happens if the player isn't in the middle of a shot       
                if (ballShot.PreparingShot == false)
                {
                    input.SwitchControlsToPauseMenu();
                    uiPauseMenu.gameObject.SetActive(true);
                    Time.timeScale = 0;
                    GamePaused = true;
                }
                break;

            case PauseEnum.Unpause:

                // If the player is on the first menu on pause menu
                if (uiPauseMenu.OnMainPauseMenu)
                {
                    UnpauseGame();
                }
                break;
        }
    }

    /// <summary>
    /// Unpauses game.
    /// </summary>
    public void UnpauseGame()
    {
        if (FindObjectOfType<BallShot>().CanShot == true)
        {
            input.SwitchControlsToGameplay();
        }
        else
        {
            input.SwitchControlsToCatapult();
        }

        uiPauseMenu.gameObject.SetActive(false);
        // ObjectEnabledCoroutine to false so it can select resume button again
        uiPauseMenu.ObjectEnabledCoroutine = null;

        // ONLY FOR TESTING
        if (TEMPFORWARD.FastForward) Time.timeScale = 2f;
        else Time.timeScale = 1f;

        GamePaused = false;
    }
}
