using UnityEngine;
using Cinemachine;
using System.Collections;
using System;

/// <summary>
/// Class responsible for controlling cinemachine cameras.
/// </summary>
public class CinemachineTarget : MonoBehaviour, IUpdateConfigurations
{
    // Values
    [SerializeField] private ConfigurationScriptableObj config;

    // Components
    [SerializeField] private CinemachineVirtualCamera ballCamera;
    private CinemachineComposer ballCameraComposer;
    [SerializeField] private CinemachineFreeLook afterShotCamera;
    [SerializeField] private CinemachineVirtualCamera courseCamera;
    private CinemachineBrain cineBrain;
    private BallHandler ball;
    private PlayerInputCustom input;

    private float xSpeedRotation;
    private float ySpeedRotation;

    // Ball fixed camera direction vertical
    private float verticalPosition;

    private void Awake()
    {
        ball = FindObjectOfType<BallHandler>();
        cineBrain = Camera.main.GetComponent<CinemachineBrain>();
        input = FindObjectOfType<PlayerInputCustom>();

        xSpeedRotation = 0;
        ySpeedRotation = 0;
    }

    private void OnEnable()
    {
        if (ball != null) ball.TypeOfMovement += SwitchCameras;
        if (ball != null) ball.Victory += VictoryCamera;
    }

    private void OnDisable()
    {
        if (ball != null) ball.TypeOfMovement -= SwitchCameras;
        if (ball != null) ball.Victory -= VictoryCamera;
    }

    private IEnumerator Start()
    {
        // Waits 0.1 seconds to let everything load first.
        yield return new WaitForSeconds(0.25f);

        // Sets course camera as first camera
        courseCamera.Priority = 20;
        cineBrain.m_DefaultBlend.m_Time = 3f;

        yield return new WaitForSeconds(2f);

        // Starts blending
        courseCamera.Priority = 0;

        yield return new WaitForSeconds(0.25f);
        while (IsCameraBlending()) 
        {
            cineBrain.m_DefaultBlend.m_Time = 3f;
            yield return null; 
        }

        // After blending is finished
        OnCameraReady();
        cineBrain.m_DefaultBlend.m_Time = 1f;       
    }

    /// <summary>
    /// If the camera loses its targets, it finds the ball's transform again.
    /// </summary>
    private void Update()
    {
        if (ball == null) ball = FindObjectOfType<BallHandler>();
        if (ballCamera.m_Follow == null && ball != null)
        {
            ballCamera.m_Follow = ball.BallPositionClone.transform;
            ballCamera.m_LookAt = ball.BallPositionClone.transform;
            ballCameraComposer = ballCamera.GetCinemachineComponent<CinemachineComposer>();
            afterShotCamera.m_Follow = ball.BallPositionClone.transform;
            afterShotCamera.m_LookAt = ball.BallPositionClone.transform;
            ball.TypeOfMovement += SwitchCameras;
        }

        // Clamps value of vertical pos y
        if (verticalPosition + input.Direction.y * config.RotationSpeed * 0.05f * Time.deltaTime < 3 &&
            verticalPosition + input.Direction.y * config.RotationSpeed * 0.05f * Time.deltaTime > 0)
        {
            verticalPosition +=
            input.Direction.y * config.RotationSpeed * 0.05f * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        // Adds target offset to ballcamera's composer
        if (ballCameraComposer != null)
        {
            if (ballCameraComposer.m_TrackedObjectOffset.y <= 3.5f &&
            ballCameraComposer.m_TrackedObjectOffset.y >= -0.5f)
            {
                ballCameraComposer.m_TrackedObjectOffset =
                    new Vector3(0, verticalPosition, 0);
            }
        }
    }

    /// <summary>
    /// Switches cameras when the player shots the ball.
    /// </summary>
    /// <param name="typeOfMovement">Current type of movement.</param>
    private void SwitchCameras(BallMovementEnum typeOfMovement)
    {
        ballCamera.Priority = 10;
        afterShotCamera.Priority = 10;
        switch (typeOfMovement)
        {
            case BallMovementEnum.Moving:
                afterShotCamera.m_YAxis.m_MaxSpeed = ySpeedRotation;
                afterShotCamera.m_XAxis.m_MaxSpeed = xSpeedRotation;
                afterShotCamera.Priority = ballCamera.Priority + 1;
                break;
            case BallMovementEnum.Stop:
                afterShotCamera.m_YAxis.m_MaxSpeed = 0;
                afterShotCamera.m_XAxis.m_MaxSpeed = 0;
                ballCamera.Priority = afterShotCamera.Priority + 1;
                break;
        }
    }

    /// <summary>
    /// Switches to course camera.
    /// </summary>
    private void VictoryCamera()
    {
        cineBrain.m_DefaultBlend.m_Time = 5;
        courseCamera.Priority = 100;
    }

    /// <summary>
    /// Method that checks if camera is blending.
    /// </summary>
    /// <returns>True if it's blending.</returns>
    public bool IsCameraBlending()
    {
        if (cineBrain.IsBlending) return true;
        return false;
    }

    /// <summary>
    /// Updates freelook camera rotation speeds.
    /// </summary>
    public void UpdateValues()
    {
        ySpeedRotation = config.FreelookVerticalRotation;
        xSpeedRotation = config.FreelookHorizontalRotation;
    }

    protected virtual void OnCameraReady() => CameraReady?.Invoke();

    /// <summary>
    /// Event registered on BallSpawn.
    /// </summary>
    public event Action CameraReady;
}
