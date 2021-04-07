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
    [SerializeField] private ConfigurationScriptableObj config;

    // Ball clone for cinemachine cameras
    [SerializeField] private Transform ballPositionClone;
    public Transform BallPositionClone => ballPositionClone;

    // Components
    private PlayerInputCustom input;
    private LineRenderer lineRenderer;
    public Rigidbody RB { get; set; }
    private CinemachineTarget cinemachine;
    private AudioSource audioSource;

    // Spawn Variables
    private Vector3 previousPosition;
    private Vector3 previousRotation;
    private Coroutine spawningCoroutine;
    private bool victory;

    // Shot variables
    public bool PreparingShot { get; set; }
    public float Power { get; set; }
    public bool FinishedCourse { get; set; }
    public bool TriggerShot { get; set; }
    private int plays;
    public bool CanShot { get; set; }

    // Movement Variables
    private float stoppedTime;
    private float stoppedTimeMax;
    private bool rotationAfterShot;
    private Vector2 direction;
    private bool isGrounded;

    // Particle variables
    [Header("Particles")]
    [SerializeField] private GameObject prefabOobParticles;
    [SerializeField] private GameObject prefabSpawnParticles;
    [SerializeField] private GameObject prefabConfettiParticles;
    [SerializeField] private GameObject hitSpaceFloorParticles;
    
    [Header("Turn on on space levels")]
    [SerializeField] private bool BezerraTempParticles;
    [Range(0.1f, 10)][SerializeField] private float particlesTime;

    // Sounds
    [Header("Sounds")]
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip groundHitSound;
    [SerializeField] private AudioClip confetiSound;
    private Coroutine playGroundHitSound;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        RB = GetComponent<Rigidbody>();
        cinemachine = FindObjectOfType<CinemachineTarget>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        lineRenderer.enabled = false;
        TriggerShot = false;
        PreparingShot = false;
        spawningCoroutine = null;
        stoppedTimeMax = 0.5f;
        rotationAfterShot = false;
        isGrounded = false;
        victory = false;
        CanShot = true;
        plays = 0;
        OnHit(plays);
    }

    private void OnEnable()
    {
        input.Shot += PrepareShot;
        input.TriggerShot += () => TriggerShot = PreparingShot;
        input.CancelShot += () => PreparingShot = false;
    }

    private void OnDisable()
    {
        input.Shot -= PrepareShot;
        input.TriggerShot -= () => TriggerShot = PreparingShot;
        input.CancelShot -= () => PreparingShot = false;
    }

    private void Update()
    {
        // Only if the camera is not blending
        if (cinemachine.IsCameraBlending() == false)
            direction = input.Direction;
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
            if (CanShot)
            {
                // Only after the ball stopped, after the shot
                if (rotationAfterShot)
                {
                    // Rotates the ball to the previous angle
                    transform.eulerAngles = previousRotation;
                    OnTypeOfMovement(BallMovementEnum.Stop);
                    rotationAfterShot = false;
                }

                // If the ball is grounded)
                if (isGrounded && victory == false)
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
    }

    /// <summary>
    /// If player isn't preparing a shot and not moving, it starts preparing a shot
    /// </summary>
    private void PrepareShot()
    {
        if (PreparingShot == false && IsStopped())
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
        while (TriggerShot == false && PreparingShot == true)
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
            yield return null;
        }

        // If the player pressed shot again, it executes shot.
        // If the ball isn't being controller by something else
        if (TriggerShot && CanShot) Shot();

        // Resets power
        Power = 0;
        PreparingShot = false;
    }

    /// <summary>
    /// If the ball is stopped, the player can shoot it.
    /// Saves current position and rotation before shot.
    /// </summary>
    public void Shot()
    {
        plays++;
        OnHit(plays);

        VisualEffect vfx = SpawnParticles(prefabSpawnParticles, 3, transform.position);
        vfx.SetFloat("Strength", Power);

        // Updates previous position
        previousPosition = new Vector3(
            transform.position.x,
            transform.position.y + 1f,
            transform.position.z);

        // Updates previous rotation
        previousRotation = transform.eulerAngles;

        PlayHitSound();
        RB.AddForce(transform.forward * Power * config.PowerMultiplier, ForceMode.Impulse);   

        // What happens the exact moment after shoting
        StartCoroutine(BehaviourAfterShot());
    }

    /// <summary>
    /// Turns rotation to true, so it can change to previous rotation after shoting.
    /// Invokes event defining the type of movement of the ball.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator BehaviourAfterShot()
    {
        yield return new WaitForFixedUpdate();
        // Event method.
        OnHit(plays);

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
        SpawnParticles(prefabOobParticles, 3, transform.position);

        yield return new WaitForSeconds(2f);

        // Stops ball's position and rotation
        StopBall();
        SpawnParticles(prefabSpawnParticles, 3, transform.position);

        // Resets ball's position
        transform.position = previousPosition;
        transform.eulerAngles = previousRotation;

        SpawnParticles(prefabSpawnParticles, 3, transform.position);

        spawningCoroutine = null;
    }

    /// <summary>
    /// Stops ball velocity and rotation.
    /// </summary>
    public void StopBall()
    {
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        RB.angularVelocity = Vector3.zero;
        RB.velocity = Vector3.zero;
        OnTypeOfMovement(BallMovementEnum.Stop);
    }

    /// <summary>
    /// Method that checks if the ball is stopped.
    /// </summary>
    /// <returns>True if it's stopped, otherwise returns false.</returns>
    private bool IsStopped()
    {
        if (RB.velocity.magnitude < 0.2f && 
            Mathf.Abs(RB.velocity.y) < 0.025f &&
            isGrounded)
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
    /// Method that happens when the ball enters the final hoje.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FinishCourse()
    {
        // Only happens once
        if (victory == false)
        {
            audioSource.PlayOneShot(confetiSound);
            SpawnParticles(prefabConfettiParticles, 6, transform.position);
            OnVictoryWithPlays(plays);
            OnVictory();
            victory = true;
        }
        while (true)
        {
            audioSource.pitch = 1f;
            yield return null;
        }
    }

    /// <summary>
    /// Spawns vfx particles.
    /// </summary>
    /// <param name="particles">Particles to spawn</param>
    /// <param name="duration">Duration of the particles.</param>
    /// <returns>Visual effect from particles.</returns>
    private VisualEffect SpawnParticles(GameObject particles, float duration, Vector3 pos)
    {
        GameObject particle = Instantiate(particles, pos, Quaternion.identity);
        Destroy(particle, duration);
        return particle.GetComponent<VisualEffect>();
    }

    /// <summary>
    /// Plays hit sound.
    /// </summary>
    private void PlayHitSound()
    {
        int randomNumber = UnityEngine.Random.Range(0, hitSounds.Length);
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(hitSounds[randomNumber]);
    }

    /// <summary>
    /// Plays ground hit sound. Has a delay so the sound doesn't repeat too often.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayGroundHitSound()
    {
        float randomNumber = UnityEngine.Random.Range(0.5f, 1f);
        audioSource.pitch = randomNumber;
        audioSource.PlayOneShot(groundHitSound);
        yield return new WaitForSeconds(2f);
        playGroundHitSound = null;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;

        if (collision.collider.CompareTag("Hole"))
        {
            StartCoroutine(FinishCourse());
        }
            
        if (collision.collider.CompareTag("OoB"))
        {
            if (spawningCoroutine == null)
                spawningCoroutine = StartCoroutine(ResetBall());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") == false)
        {
            // Only happens after a while, so the sound doesn't spam
            if (playGroundHitSound == null)
            {
                playGroundHitSound = StartCoroutine(PlayGroundHitSound());
            }
        }

        if (collision.collider.CompareTag("Ground"))
        {
            if (collision.relativeVelocity.y > 0.3f)
            {
                // Only happens after a while, so the sound doesn't spam
                if (playGroundHitSound == null)
                {
                    playGroundHitSound = StartCoroutine(PlayGroundHitSound());
                }

                if (BezerraTempParticles)
                {
                    SpawnParticles(
                        hitSpaceFloorParticles,
                        particlesTime, 
                        collision.contacts[0].point);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }    
    }

    protected virtual void OnTypeOfMovement(BallMovementEnum movement) => 
        TypeOfMovement?.Invoke(movement);

    /// <summary>
    /// Event registered on CinemachineTarget.
    /// </summary>
    public event Action<BallMovementEnum> TypeOfMovement;


    protected virtual void OnVictoryWithPlays(int numOfPlays) => 
        VictoryWithPlays?.Invoke(numOfPlays);

    /// <summary>
    /// Event registered on LevelPassed.
    /// Event registered on HighscoreHandler.
    /// </summary>
    public event Action<int> VictoryWithPlays;

    protected virtual void OnVictory() =>
        Victory?.Invoke();

    /// <summary>
    /// Event registered on CinemachineTarget.
    /// </summary>
    public event Action Victory;

    protected virtual void OnHit(int score) => Hit?.Invoke(score);

    /// <summary>
    /// Event registered on UIShots;
    /// </summary>
    public event Action<int> Hit;
}
