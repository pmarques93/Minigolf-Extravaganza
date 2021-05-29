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
    private BallSounds ballSounds;
    public LineRenderer LineRenderer { get; set; }
    public float LineYValue { get; set; }
    [SerializeField] private LayerMask lineHitLayers;

    private void Awake()
    {
        ballShot = GetComponent<BallShot>();
        LineRenderer = GetComponentInChildren<LineRenderer>();
        ballSounds = GetComponent<BallSounds>();
    }

    private void Start()
    {
        Victory = false;
        LineRenderer.enabled = false;
    }

    /// <summary>
    /// Draws line renderer from ball to forward.
    /// </summary>
    public void DrawLine()
    {
        LineRenderer.SetPosition(0, transform.position);

        Ray lineForward = new Ray(transform.position, LineRenderer.GetPosition(1) - transform.position);
        Ray normalForward = 
            new Ray(transform.position, 
            new Vector3(LineRenderer.GetPosition(1).x, LineRenderer.GetPosition(1).y - 0.2f, LineRenderer.GetPosition(1).z) - transform.position);

        // Increments final line's point Y, so it goes up
        if (Physics.Raycast(lineForward, config.LineLength, lineHitLayers) != false)
        {
            LineYValue += 0.1f;
        }
        else
        {
            // If a ray below the current ray would hit something
            if (Physics.Raycast(normalForward, config.LineLength, lineHitLayers) != false)
            {
                // Does nothing
            }
            // If it wouldn't hit something
            else
            {
                // Puts line back to normal 
                if (LineYValue > 0)
                    LineYValue -= 0.1f;
            }
        }

        LineRenderer.SetPosition(1, 
            new Vector3(transform.position.x, transform.position.y + LineYValue * 1.2f, transform.position.z) + 
            transform.forward * config.LineLength);
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
