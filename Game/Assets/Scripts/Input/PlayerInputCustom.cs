using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;

public class PlayerInputCustom : MonoBehaviour
{
    // Components
    [SerializeField] private PlayerInput inputControl;
    [SerializeField] private BaseInputModule inputModule;

    // Components
    private PauseMenu pauseMenu;

    // Getters
    public Vector2 Direction { get; private set; }

    private void Awake()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SwitchControlsToPauseMenu();
    }

    public void SwitchControlsToGameplay() => 
        inputControl.SwitchCurrentActionMap("Gameplay");

    public void SwitchControlsToPauseMenu() =>
        inputControl.SwitchCurrentActionMap("PauseMenu");

    public void SwitchControlsToCatapult() =>
        inputControl.SwitchCurrentActionMap("Catapult");

    protected virtual void OnChangeControlsToCatapult() =>
        ChangeControlsToCatapult?.Invoke();

    public event Action ChangeControlsToCatapult;

    /// <summary>
    /// Disables UI input module.
    /// </summary>
    public void SwitchControlsToDisableOnMenu()
    {
        inputControl.SwitchCurrentActionMap("DisableControls");
        inputModule.enabled = false;
    }

    /// <summary>
    /// Handles direction.
    /// </summary>
    /// <param name="context"></param>
    public void HandleDirection(InputAction.CallbackContext context)
    {
        if (context.performed) Direction = context.ReadValue<Vector2>();
        if (context.canceled)
        {
            Direction = new Vector2Int(0, 0);
        }
    }

    /// <summary>
    /// Handles shot.
    /// </summary>
    /// <param name="context"></param>
    public void HandleShot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnShot();
            OnTriggerShot();
        }
    }

    protected virtual void OnTriggerShot() => TriggerShot?.Invoke();
    protected virtual void OnShot() => Shot?.Invoke();

    /// <summary>
    /// Event registered on BallHandler.
    /// </summary>
    public event Action TriggerShot;

    /// <summary>
    /// Event registered on BallHandler.
    /// </summary>
    public event Action Shot;

    /// <summary>
    /// Handles shot.
    /// </summary>
    /// <param name="context"></param>
    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Check has to happen before canceling shot, so it doesn't cancel
            // and pause at the same time
            if (pauseMenu.GamePaused == false)
            {
                OnPauseGame(PauseEnum.Pause);
            }

            // Then it cancels shot, if player is preparing a shot
            OnCancelShot();
        }
    }

    protected virtual void OnCancelShot() => CancelShot?.Invoke();

    /// <summary>
    /// Event registered on BallHandler.
    /// </summary>
    public event Action CancelShot;

    //////////////////////////////// CATAPULT //////////////////////////////////
    /// Handles direction.
    /// </summary>
    /// <param name="context"></param>
    public void HandleDirectionCatapult(InputAction.CallbackContext context)
    {
        if (context.performed) Direction = context.ReadValue<Vector2>();
        if (context.canceled)
        {
            Direction = new Vector2Int(0, 0);
        }
    }

    /// <summary>
    /// Handles shot.
    /// </summary>
    /// <param name="context"></param>
    public void HandleShotCatapult(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnShotCatapult();
            OnTriggerShotCatapult();
        }
    }

    protected virtual void OnTriggerShotCatapult() => TriggerShotCatapult?.Invoke();
    protected virtual void OnShotCatapult() => ShotCatapult?.Invoke();

    /// <summary>
    /// Event registered on BallHandler.
    /// </summary>
    public event Action TriggerShotCatapult;

    /// <summary>
    /// Event registered on BallHandler.
    /// </summary>
    public event Action ShotCatapult;

    /// <summary>
    /// Handles shot.
    /// </summary>
    /// <param name="context"></param>
    public void HandleCancelCatapult(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Check has to happen before canceling shot, so it doesn't cancel
            // and pause at the same time
            if (pauseMenu.GamePaused == false)
            {
                OnPauseGame(PauseEnum.Pause);
            }

            // Then it cancels shot, if player is preparing a shot
            OnCancelShotCatapult();
        }
    }

    protected virtual void OnCancelShotCatapult() => CancelShotCatapult?.Invoke();

    /// <summary>
    /// Event registered on BallHandler.
    /// </summary>
    public event Action CancelShotCatapult;

    ////////////////////////////////// UI //////////////////////////////////////

    /// <summary>
    /// Handles confirmation.
    /// </summary>
    /// <param name="context"></param>
    public void HandleConfirmation(InputAction.CallbackContext context)
    {
        if (context.started) OnConfirm();
    }

    protected virtual void OnConfirm() => Confirm?.Invoke();

    public event Action Confirm;

    /// <summary>
    /// Handles pause.
    /// </summary>
    /// <param name="context"></param>
    public void HandlePause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnPauseGame(PauseEnum.Unpause);
        }
    }

    protected virtual void OnPauseGame(PauseEnum pauseType) => 
        PauseGame?.Invoke(pauseType);

    /// <summary>
    /// Event registered on PauseMenu.
    /// </summary>
    public event Action<PauseEnum> PauseGame;

}
