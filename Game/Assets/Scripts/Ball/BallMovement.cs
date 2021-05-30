using System.Collections;
using UnityEngine;
using System;

public class BallMovement : MonoBehaviour
{
    // Movement Variables
    public bool IsGrounded { get; set; }
    public bool RotationAfterShot { get; set; }
    private float stoppedTime;
    private float stoppedTimeMax;

    // Previous transform variables
    private Vector3 previousPosition;
    private Vector3 previousRotation;
    public bool SpawningCoroutine { get; set; }

    // Ball clone for cinemachine cameras
    [SerializeField] private Transform ballPositionClone;
    public Transform BallPositionClone => ballPositionClone;

    // Components
    public Rigidbody Rb { get; private set; }
    private BallHandler ballHandler;
    private PlayerInputCustom input;
    private BallShot ballShot;
    
    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        ballHandler = GetComponent<BallHandler>();
        input = FindObjectOfType<PlayerInputCustom>();
        ballShot = GetComponent<BallShot>();
        
    }

    private void Start()
    {
        stoppedTimeMax = 0.5f;
        RotationAfterShot = false;
        IsGrounded = false;
        SpawningCoroutine = false;
        
    }

    private void FixedUpdate()
    {
        // Updates ball's clone position
        ballPositionClone.position = transform.position;
        // Only if the ball is stopped
        if (IsStopped())
        {
            // Updates ball's clone rotation
            ballPositionClone.rotation = transform.rotation;

            // If the ball isn't being controller by something else
            if (ballShot.CanShot)
            {
                // Only after the ball stopped, after the shot
                if (RotationAfterShot)
                {
                    // Rotates the ball to the previous angle
                    transform.eulerAngles = previousRotation;
                    RotationAfterShot = false;
                }

                // Rotates the ball with player's input
                transform.eulerAngles +=
                    new Vector3(0f, input.Direction.x, 0f) * Time.fixedDeltaTime * ballHandler.Config.RotationSpeed;

                // Renders line renderer
                ballHandler.DrawLine();
            }
        }
    }

    /// <summary>
    /// Method that checks if the ball is stopped.
    /// </summary>
    /// <returns>True if it's stopped, otherwise returns false.</returns>
    public bool IsStopped()
    {
        if (Rb.velocity.magnitude < 0.2f &&
            Mathf.Abs(Rb.velocity.y) < 0.025f &&
            IsGrounded)
        {
            stoppedTime += Time.fixedDeltaTime;
        }
        else
        {
            stoppedTime = 0;
        }

        if (stoppedTime >= stoppedTimeMax)
        {
            StopBall();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Stops ball velocity and rotation.
    /// </summary>
    public void StopBall()
    {
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        Rb.angularVelocity = Vector3.zero;
        Rb.velocity = Vector3.zero;
        OnTypeOfMovement(BallMovementEnum.Stop);
    }

    /// <summary>
    /// Saves current transform + offset.
    /// </summary>
    /// <param name="pos">Ball's position.</param>
    /// <param name="rot">Ball's rotation.</param>
    public void SavePositionAndRotation(Vector3 pos, Vector3 rot)
    {
        previousPosition = new Vector3(pos.x, pos.y + 1f, pos.z);
        previousRotation = rot;
    }

    /// <summary>
    /// Resets ball. Happens when the ball hits the out bounds.
    /// </summary>
    /// <returns>Returns null.</returns>
    public IEnumerator ResetBall()
    {
        // Spawns particles when the ball hits out of bounds.
        ballHandler.SpawnParticles(ballHandler.PrefabOobParticles, 3, transform.position);

        yield return new WaitForSeconds(2f);

        // Stops ball's position and rotation
        StopBall();
        ballHandler.SpawnParticles(ballHandler.PrefabSpawnParticles, 3, transform.position);

        // Resets ball's position
        transform.position = previousPosition;
        transform.eulerAngles = previousRotation;

        ballHandler.SpawnParticles(ballHandler.PrefabSpawnParticles, 3, transform.position);

        SpawningCoroutine = false;
    }

    public virtual void OnTypeOfMovement(BallMovementEnum movement) =>
        TypeOfMovement?.Invoke(movement);

    /// <summary>
    /// Event registered on CinemachineTarget.
    /// </summary>
    public event Action<BallMovementEnum> TypeOfMovement;
}
