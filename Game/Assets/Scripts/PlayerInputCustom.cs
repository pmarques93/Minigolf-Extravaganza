using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputCustom : MonoBehaviour
{
    [SerializeField] private PlayerInput inputControl;

    public Vector2 Direction { get; private set; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

    public event Action TriggerShot;

    public event Action Shot;



    /// <summary>
    /// Handles shot.
    /// </summary>
    /// <param name="context"></param>
    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (context.started) OnCancelShot();
    }

    protected virtual void OnCancelShot() => CancelShot?.Invoke();

    public event Action CancelShot;
}
