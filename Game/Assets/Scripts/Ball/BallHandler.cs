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
    [SerializeField] private bool bezerraTempParticles;
    public bool BezerraTempParticles => bezerraTempParticles;

    // Components
    private BallShot ballShot;
    private BallSounds ballSounds;
    public LineRenderer LineRenderer { get; set; }


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
        LineRenderer.SetPosition(0, transform.position + transform.forward * 0.4f);
        LineRenderer.SetPosition(1, transform.position + transform.forward * config.LineLength);
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
