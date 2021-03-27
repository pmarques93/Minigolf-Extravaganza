using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class responsible for handling ball behaviour.
/// </summary>
public class BallHandler : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float lineLength;
    [SerializeField] private float powerTime;
    [SerializeField] private float powerMultiplier;

    [SerializeField] private Transform ballPositionClone;
    public Transform BallPositionClone => ballPositionClone;

    // Components
    private PlayerInputCustom input;
    private LineRenderer lineRenderer;
    private Rigidbody rb;

    // Spawn Variables
    private Vector3 previousPosition;
    private Vector3 previousRotation;
    private Coroutine spawningCoroutine;

    // Shot variables
    public bool PreparingShot { get; private set; }
    private bool triggerShot;
    public float Power { get; private set; }
    public int Plays { get; private set; }
    public bool FinishedCourse { get; set; }

    // Movement Variables
    private float stoppedTime;
    private float stoppedTimeMax;
    private bool rotationAfterShot;
    private Vector2 direction;

    // Particle variables
    [SerializeField] private GameObject prefabOobParticles;
    [SerializeField] private GameObject prefabSpawnParticles;
    [SerializeField] private GameObject prefabConfettiParticles;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        triggerShot = false;
        PreparingShot = false;
        spawningCoroutine = null;
        stoppedTimeMax = 0.25f;
        rotationAfterShot = false;
    }

    private void OnEnable()
    {
        input.Shot += PrepareShot;
        input.TriggerShot += () => triggerShot = PreparingShot ? true : false;
        input.CancelShot += () => PreparingShot = false;
    }

    private void OnDisable()
    {
        input.Shot -= PrepareShot;
        input.TriggerShot -= () => triggerShot = PreparingShot ? true : false;
        input.CancelShot -= () => PreparingShot = false;
    }

    private void Update()
    {
        direction = input.Direction;
    }

    private void FixedUpdate()
    {
        ballPositionClone.position = transform.position;
        if (IsStopped())
        {
            ballPositionClone.rotation = transform.rotation;
            if (rotationAfterShot)
            {
                // Rotates the ball to the previous angle
                transform.eulerAngles = previousRotation;
                lineRenderer.enabled = true;
                rotationAfterShot = false;
                OnTypeOfMovement(BallMovementEnum.Stop);
            }

            transform.eulerAngles +=
                new Vector3(0f, direction.x, 0f) * Time.fixedDeltaTime * rotationSpeed;

            // Renders line renderer
            DrawLine();
        }
    }

    /// <summary>
    /// If player isn't preparing a shot and not moving, it starts preparing a shot
    /// </summary>
    private void PrepareShot()
    {
        if (PreparingShot == false && rb.velocity == Vector3.zero)
        {
            StartCoroutine(PrepareShotForce(powerTime));
        }
    }

    /// <summary>
    /// While the player doesn't trigger shot, the power will keep changing
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrepareShotForce(float powerTime)
    {
        yield return new WaitForFixedUpdate();
        PreparingShot = true;

        bool powerGrowing = true;
        while (triggerShot == false && PreparingShot == true)
        {
            // Increments the power value
            if (powerGrowing)
            {
                Power += Time.fixedDeltaTime * powerTime;
                if (Power >= 1) powerGrowing = false;
            }
            // Decrements the power value
            else
            {
                Power -= Time.fixedDeltaTime * powerTime;
                if (Power <= 0) powerGrowing = true;
            }
            
            Mathf.PingPong(Power, 1f);
            yield return null;
        }

        // If the player pressed shot again, it executes shot.
        if (triggerShot) Shoot();

        // Resets power
        Power = 0;
        PreparingShot = false;
    }

    /// <summary>
    /// If the ball is stopped, the player can shoot it.
    /// </summary>
    private void Shoot()
    {
        lineRenderer.enabled = false;

        previousPosition = new Vector3(
            transform.position.x,
            transform.position.y + 1f,
            transform.position.z);

        previousRotation = transform.eulerAngles;

        rb.AddForce(transform.forward * Power * powerMultiplier, ForceMode.Impulse);

        OnTypeOfMovement(BallMovementEnum.Moving);
        StartCoroutine(RotationAfterShotToTrue());
    }

    /// <summary>
    /// Turns rotation to true, so it can change to previous rotation after shoting.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator RotationAfterShotToTrue()
    {
        yield return new WaitForFixedUpdate();
        rotationAfterShot = true;
    }

    /// <summary>
    /// Draws line renderer from ball to forward.
    /// </summary>
    private void DrawLine()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * lineLength);
    }

    /// <summary>
    /// Resets ball. Happens when the ball hits the out bounds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetBall()
    {
        yield return new WaitForSeconds(2f);

        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;

        transform.position = previousPosition;
        transform.eulerAngles = previousRotation;
        spawningCoroutine = null;
    }

    private bool IsStopped()
    {
        if (rb.velocity == Vector3.zero)
        {
            stoppedTime += Time.time;
        }
        else
        {
            stoppedTime = 0;
        }

        if (stoppedTime >= stoppedTimeMax)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("OoB"))
        {
            if (spawningCoroutine == null)
                spawningCoroutine = StartCoroutine(ResetBall());
        }
    }

    protected virtual void OnTypeOfMovement(BallMovementEnum movement) => 
        TypeOfMovement?.Invoke(movement);

    /// <summary>
    /// Event registered on CinemachineTarget.
    /// </summary>
    public event Action<BallMovementEnum> TypeOfMovement;
}
