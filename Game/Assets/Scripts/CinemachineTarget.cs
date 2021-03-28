using UnityEngine;
using Cinemachine;
using System.Collections;
using System;

/// <summary>
/// Class responsible for controlling cinemachine cameras.
/// </summary>
public class CinemachineTarget : MonoBehaviour
{
    // Components
    [SerializeField] private CinemachineVirtualCamera ballCamera;
    [SerializeField] private CinemachineFreeLook afterShotCamera;
    [SerializeField] private CinemachineVirtualCamera courseCamera;
    private CinemachineBrain cineBrain;
    private BallHandler ball;

    private void Awake()
    {
        ball = FindObjectOfType<BallHandler>();
        cineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void OnEnable()
    {
        if (ball != null) ball.TypeOfMovement += SwitchCameras;
    }

    private void OnDisable()
    {
        if (ball != null) ball.TypeOfMovement -= SwitchCameras;
    }

    private IEnumerator Start()
    {
        // Waits 0.1 seconds to let everything load first.
        yield return new WaitForSeconds(0.25f);

        courseCamera.Priority = 20;
        cineBrain.m_DefaultBlend.m_Time = 3f;

        yield return new WaitForSeconds(2f);

        // Starts blending
        courseCamera.Priority = 0;

        yield return new WaitForFixedUpdate();
        while (cineBrain.IsBlending == true) 
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

    protected virtual void OnCameraReady() => CameraReady?.Invoke();

    /// <summary>
    /// Event registered on BallSpawn.
    /// </summary>
    public event Action CameraReady;
}
