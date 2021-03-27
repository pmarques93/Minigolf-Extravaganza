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
    private BallHandler ball;
    [SerializeField] private UIPauseMenu uiPauseMenu;

    // Pause variables
    public bool GamePaused { get; private set; }

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        ball = FindObjectOfType<BallHandler>();
    }

    private void Start()
    {
        GamePaused = false;
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
        if (ball == null) ball = FindObjectOfType<BallHandler>();
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
                if (ball.PreparingShot == false)
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
                    input.SwitchControlsToGameplay();

                    uiPauseMenu.gameObject.SetActive(false);
                    // ObjectEnabledCoroutine to false so it can select resume button again
                    uiPauseMenu.ObjectEnabledCoroutine = null;

                    Time.timeScale = 1f;
                    GamePaused = false;
                }
                break;
        }
    }
}
