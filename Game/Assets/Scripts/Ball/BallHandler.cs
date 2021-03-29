using System.Collections;
using UnityEngine;
using System;
using UnityEngine.VFX;

/// <summary>
/// Class responsible for handling ball behaviour.
/// </summary>
public class BallHandler : MonoBehaviour
{
    // Values
    [SerializeField] ConfigurationScriptableObj config;

    // Ball clone for cinemachine cameras
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
    private bool victory;

    // Shot variables
    public bool PreparingShot { get; private set; }
    private bool triggerShot;
    public float Power { get; set; }
    public int Plays { get; private set; }
    public bool FinishedCourse { get; set; }

    // Movement Variables
    private float stoppedTime;
    private float stoppedTimeMax;
    private bool rotationAfterShot;
    private Vector2 direction;
    private bool isGrounded;

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
        lineRenderer.enabled = false;
        triggerShot = false;
        PreparingShot = false;
        spawningCoroutine = null;
        stoppedTimeMax = 0.25f;
        rotationAfterShot = false;
        isGrounded = false;
        victory = false;
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
        // Updates ball's clone position
        ballPositionClone.position = transform.position;
        if (IsStopped())
        {
            // Updates ball's clone rotation
            ballPositionClone.rotation = transform.rotation;

            // After the ball stopped, after the shot
            if (rotationAfterShot)
            {
                // Rotates the ball to the previous angle
                transform.eulerAngles = previousRotation;
                OnTypeOfMovement(BallMovementEnum.Stop);
                rotationAfterShot = false;
            }

            // If the ball is grounded)
            if (isGrounded)
            {
                if (lineRenderer.enabled == false)
                    lineRenderer.enabled = true;
            }

            // Rotates the ball with player's input
            transform.eulerAngles +=
                new Vector3(0f, direction.x, 0f) * Time.fixedDeltaTime * config.RotationSpeed;

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
            StartCoroutine(PrepareShotForce(config.PowerTime));
        }
    }

    /// <summary>
    /// While the player doesn't trigger shot, the power will keep changing.
    /// </summary>
    /// <returns>Wait for Fixed Update</returns>
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
        if (triggerShot) Shot();

        // Resets power
        Power = 0;
        PreparingShot = false;
    }

    /// <summary>
    /// If the ball is stopped, the player can shoot it.
    /// Saves current position and rotation before shot.
    /// </summary>
    private void Shot()
    {
        VisualEffect vfx = SpawnParticles(prefabSpawnParticles, 3);
        vfx.SetFloat("Strength", Power);

        // Updates previous position
        previousPosition = new Vector3(
            transform.position.x,
            transform.position.y + 1f,
            transform.position.z);

        // Updates previous rotation
        previousRotation = transform.eulerAngles;

        rb.AddForce(transform.forward * Power * config.PowerMultiplier, ForceMode.Impulse);

        // What happens the exact moment after shoting
        StartCoroutine(RotationAfterShotToTrue());
    }

    /// <summary>
    /// Turns rotation to true, so it can change to previous rotation after shoting.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator RotationAfterShotToTrue()
    {
        yield return new WaitForFixedUpdate();
        // Event method.
        OnShotHit();

        SpawnParticles(prefabOobParticles, 3);
        OnTypeOfMovement(BallMovementEnum.Moving);
        lineRenderer.enabled = false;
        rotationAfterShot = true;
    }

    /// <summary>
    /// Draws line renderer from ball to forward.
    /// </summary>
    private void DrawLine()
    {
        lineRenderer.SetPosition(0, transform.position + transform.forward * 0.4f);
        lineRenderer.SetPosition(1, transform.position + transform.forward * config.LineLength);
    }

    /// <summary>
    /// Resets ball. Happens when the ball hits the out bounds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetBall()
    {
        // Spawns particles when the ball hits out of bounds.
        SpawnParticles(prefabOobParticles, 3);

        yield return new WaitForSeconds(2f);

        // Stops ball's position and rotation
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero; 

        SpawnParticles(prefabSpawnParticles, 3);

        // Resets ball's position
        transform.position = previousPosition;
        transform.eulerAngles = previousRotation;

        SpawnParticles(prefabSpawnParticles, 3);

        spawningCoroutine = null;
    }

    /// <summary>
    /// Method that checks if the ball is stopped.
    /// </summary>
    /// <returns>True if it's stopped, otherwise returns false.</returns>
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

    /// <summary>
    /// Method that happens when the ball enters the final hoje.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FinishCourse()
    {
        yield return new WaitForSeconds(1f);

        if (victory == false)
        {
            SpawnParticles(prefabConfettiParticles, 6);
            OnVictory();
            gameObject.SetActive(false);
            victory = true;
        }
    }

    /// <summary>
    /// Spawns vfx particles.
    /// </summary>
    /// <param name="particles">Particles to spawn</param>
    /// <param name="duration">Duration of the particles.</param>
    /// <returns>Visual effect from particles.</returns>
    private VisualEffect SpawnParticles(GameObject particles, float duration)
    {
        GameObject particle = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(particle, duration);
        return particle.GetComponent<VisualEffect>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;

        if (collision.collider.CompareTag("Hole"))
            StartCoroutine(FinishCourse());

        if (collision.collider.CompareTag("OoB"))
        {
            if (spawningCoroutine == null)
                spawningCoroutine = StartCoroutine(ResetBall());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }

    protected virtual void OnTypeOfMovement(BallMovementEnum movement) => 
        TypeOfMovement?.Invoke(movement);

    /// <summary>
    /// Event registered on CinemachineTarget.
    /// </summary>
    public event Action<BallMovementEnum> TypeOfMovement;

    protected virtual void OnShotHit() => ShotHit?.Invoke();

    /// <summary>
    /// Event registered on BallScore.
    /// </summary>
    public event Action ShotHit;

    protected virtual void OnVictory() => Victory?.Invoke();

    /// <summary>
    /// Event registered on LevelPassed.
    /// </summary>
    public event Action Victory;
}
