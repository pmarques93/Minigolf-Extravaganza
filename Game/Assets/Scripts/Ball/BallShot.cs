using System.Collections;
using UnityEngine;
using System;
using UnityEngine.VFX;

/// <summary>
/// Class responsible for handling ball's shots.
/// </summary>
public class BallShot : MonoBehaviour
{
    public bool PreparingShot { get; set; }
    public float Power { get; set; }
    public bool TriggerShot { get; set; }
    public bool CanShot { get; set; }
    public int Plays { get; set; }

    // Components
    private PlayerInputCustom input;
    private BallHandler ballHandler;
    private BallMovement ballMovement;
    private BallSounds ballSounds;
    private Rigidbody rb;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        ballHandler = GetComponent<BallHandler>();
        ballMovement = GetComponent<BallMovement>();
        ballSounds = GetComponent<BallSounds>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        TriggerShot = false;
        PreparingShot = false;
        CanShot = true;
        Plays = 0;
        OnHit(Plays);
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

    /// <summary>
    /// If player isn't preparing a shot and not moving, it starts preparing a shot
    /// </summary>
    private void PrepareShot()
    {
        if (PreparingShot == false && ballMovement.IsStopped())
        {
            StartCoroutine(PrepareShotForce(ballHandler.Config.PowerTime));
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
        Plays++;
        OnHit(Plays);

        // Resets line renderer final point back to 0
        ballHandler.LineYValue = 0;
        ballHandler.FinalLineLength = ballHandler.LineLength;

        VisualEffect vfx = 
            ballHandler.SpawnParticles(
                ballHandler.PrefabSpawnParticles, 3, transform.position);
        vfx.SetFloat("Strength", Power);

        ballMovement.SavePositionAndRotation(
            transform.position, transform.eulerAngles);

        ballSounds.PlaySound(BallSoundEnum.HitSound);

        rb.AddForce(
            transform.forward * Power * ballHandler.Config.PowerMultiplier, ForceMode.Impulse);

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
        OnHit(Plays);

        ballMovement.OnTypeOfMovement(BallMovementEnum.Moving);
        ballHandler.LineRenderer.enabled = false;
        ballMovement.RotationAfterShot = true;
    }


    protected virtual void OnHit(int score) => Hit?.Invoke(score);

    /// <summary>
    /// Event registered on UIShots;
    /// </summary>
    public event Action<int> Hit;
}
