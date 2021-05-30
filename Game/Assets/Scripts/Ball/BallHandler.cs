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
    public ConfigurationScriptableObj Config => config;

    public bool Victory { get; set; }
    
    [Header("Particles")]
    [SerializeField] private GameObject prefabOobParticles;
    public GameObject PrefabOobParticles => prefabOobParticles;

    [SerializeField] private GameObject prefabSpawnParticles;
    public GameObject PrefabSpawnParticles => prefabSpawnParticles;

    [SerializeField] private GameObject prefabConfettiParticles;
    public GameObject PrefabConfettiParticles => prefabConfettiParticles;

    [SerializeField] private GameObject prefabLightBridgeCollision;
    public GameObject PrefabLightBridgeCollision => prefabLightBridgeCollision;

    [Header("Turn on on space levels")]
    [SerializeField] private bool spaceParticles;
    public bool SpaceParticles => spaceParticles;

    // Components
    private BallShot ballShot;
    private BallMovement ballMovement;
    private BallSounds ballSounds;
    private CinemachineTarget cinemachine;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject lineRendererObject;
    private float lineYValue;
    [SerializeField] private Transform linePosition;
    private Coroutine activateLineRenderer;

    private void Awake()
    {
        ballShot = GetComponent<BallShot>();
        ballMovement = GetComponent<BallMovement>();
        ballSounds = GetComponent<BallSounds>();
        cinemachine = FindObjectOfType<CinemachineTarget>();
    }

    private void Start()
    {
        Victory = false;
        lineRendererObject.SetActive(false);
        lineYValue = 1f;
    }

    private void Update()
    {
        if (cinemachine.IsCameraBlending())
        {
            if (lineRendererObject.activeSelf == true)
            {
                lineRendererObject.SetActive(false);
            }

            activateLineRenderer = null;
        }
        else if (cinemachine.IsCameraBlending() == false && 
            cinemachine.BallCamera.Priority > cinemachine.AfterShotCamera.Priority &&
            ballMovement.IsGrounded &&
            Victory == false)
        {
            if (activateLineRenderer == null)
                activateLineRenderer = StartCoroutine(ActivateLineRenderer()); 
        }
    }

    private IEnumerator ActivateLineRenderer()
    {
        yield return new WaitForFixedUpdate();
        lineRendererObject.SetActive(true);
    }

    /// <summary>
    /// Draws line renderer from ball to forward.
    /// </summary>
    public void DrawLine()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, linePosition.position);

            lineRenderer.SetPosition(1,
                new Vector3(linePosition.position.x, linePosition.position.y + lineYValue, linePosition.position.z));
        }
    }

    /// <summary>
    /// Method that happens when the ball enters the final hoje.
    /// </summary>
    /// <returns></returns>
    public IEnumerator FinishCourse()
    {
        // Only happens once
        if (Victory == false)
        {
            ballSounds.PlaySound(BallSoundEnum.ConfetiSound);
            SpawnParticles(prefabConfettiParticles, 6, transform.position);
            OnVictoryWithPlays(ballShot.Plays);
            OnVictoryEvent();
            Victory = true;
        }
        while (true)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Spawns vfx particles.
    /// </summary>
    /// <param name="particles">Particles to spawn</param>
    /// <param name="duration">Duration of the particles.</param>
    /// <returns>Visual effect from particles.</returns>
    public VisualEffect SpawnParticles(GameObject particles, float duration, Vector3 pos)
    {
        GameObject particle = Instantiate(particles, pos, Quaternion.identity);
        Destroy(particle, duration);
        return particle.GetComponent<VisualEffect>();
    }

    protected virtual void OnVictoryWithPlays(int numOfPlays) => 
        VictoryWithPlays?.Invoke(numOfPlays);

    /// <summary>
    /// Event registered on LevelPassed.
    /// Event registered on HighscoreHandler.
    /// </summary>
    public event Action<int> VictoryWithPlays;

    protected virtual void OnVictoryEvent() =>
        VictoryEvent?.Invoke();

    /// <summary>
    /// Event registered on CinemachineTarget.
    /// </summary>
    public event Action VictoryEvent;

    
}
