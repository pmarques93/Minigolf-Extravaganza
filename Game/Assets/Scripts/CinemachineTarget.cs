using UnityEngine;
using Cinemachine;

/// <summary>
/// Class responsible for controlling cinemachine cameras.
/// </summary>
public class CinemachineTarget : MonoBehaviour
{
    // Components
    [SerializeField] private CinemachineVirtualCamera ballCamera;
    [SerializeField] private CinemachineFreeLook afterShotCamera;
    private BallHandler ball;

    private void Awake()
    {
        ball = FindObjectOfType<BallHandler>();
    }

    private void OnEnable()
    {
        if (ball != null) ball.TypeOfMovement += SwitchCameras;
    }

    private void OnDisable()
    {
        if (ball != null) ball.TypeOfMovement -= SwitchCameras;
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
            afterShotCamera.m_Follow = ball.BallPositionClone.transform;
            afterShotCamera.m_LookAt = ball.BallPositionClone.transform;
            ball.TypeOfMovement += SwitchCameras;
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
                afterShotCamera.Priority = ballCamera.Priority + 1;
                break;
            case BallMovementEnum.Stop:
                ballCamera.Priority = afterShotCamera.Priority + 1;
                break;
        }
    }
}
