using UnityEngine;

/// <summary>
/// Class responsible for what happens after the ball spawned.
/// </summary>
public class BallSpawn : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;
    private CinemachineTarget cinemachine;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        cinemachine = FindObjectOfType<CinemachineTarget>();
    }

    private void OnEnable()
    {
        cinemachine.CameraReady += EnableControls;
    }

    private void OnDisable()
    {
        cinemachine.CameraReady -= EnableControls;
    }

    /// <summary>
    /// Enables controls when camera ends blending.
    /// </summary>
    private void EnableControls()
    {
        input.SwitchControlsToGameplay();
    }
}
