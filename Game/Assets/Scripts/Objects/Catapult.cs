using UnityEngine;
using Cinemachine;

/// <summary>
/// Class responsible for handling catapult control.
/// </summary>
public class Catapult : MonoBehaviour
{
    [SerializeField] private ConfigurationScriptableObj config;
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Transform ballPosition;
    [Range(0.25f,2f)][SerializeField] private float ballForce;
    public float EndBallForce { get; private set; }

    // Components
    private PlayerInputCustom input;
    private CinemachineTarget cinemachine;
    private Animator anim;

    // Ball control variables
    public bool HasBall { get; private set; }
    private BallMovement ballMovement;
    private BallShot ballShot;
    private Vector2 direction;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        cinemachine = FindObjectOfType<CinemachineTarget>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        input.ShotCatapult += Shot;
    }
    private void OnDisable()
    {
        input.ShotCatapult -= Shot;
    }

    private void Start()
    {
        HasBall = false;

        EndBallForce = ballForce * config.PowerMultiplier;
    }

    private void Update()
    {
        if (cinemachine.IsCameraBlending() == false)
            direction = input.Direction;
    }

    /// <summary>
    /// Controls catapult rotation.
    /// </summary>
    private void FixedUpdate()
    {
        if (HasBall)
        {
            ballMovement.StopBall();
            ballMovement.transform.position = ballPosition.position;

            // Rotates the catapult with player's input
            transform.eulerAngles +=
                new Vector3(0f, direction.x, 0f) * Time.fixedDeltaTime * config.RotationSpeed * 0.25f;
        }
    }

    /// <summary>
    /// Triggers shot animation.
    /// </summary>
    private void Shot()
    {
        anim.SetTrigger("Throw");
    }

    /// <summary>
    /// Cancels all ball movement and sets camera.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BallCollisions>(out BallCollisions ball))
        {
            ballMovement = ball.gameObject.GetComponent<BallMovement>();
            ballShot = ball.gameObject.GetComponent<BallShot>();

            cam.Priority = 100;
            
            HasBall = true;
            ballShot.CanShot = false;

            input.SwitchControlsToCatapult();   
        }
    }

    /// <summary>
    /// Event called on animation event.
    /// Shots the ball and gives back player control to ball.
    /// </summary>
    public void ThrowBall()
    {
        cam.Priority = 0;

        HasBall = false;
        ballShot.CanShot = true;

        ballMovement.Rb.AddForce(transform.forward * ballForce * config.PowerMultiplier, ForceMode.Impulse);

        ballMovement.OnTypeOfMovement(BallMovementEnum.Moving);

        input.SwitchControlsToGameplay();

    }
}
