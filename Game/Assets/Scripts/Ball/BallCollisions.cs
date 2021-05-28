using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling ball's collisions.
/// </summary>
public class BallCollisions : MonoBehaviour
{
    private BallHandler ballHandler;
    private BallMovement ballMovement;
    private BallSounds ballSounds;

    private Coroutine playGroundHitSound;

    private void Awake()
    {
        ballHandler = GetComponent<BallHandler>();
        ballMovement = GetComponent<BallMovement>();
        ballSounds = GetComponent<BallSounds>();
    }

    private void Start()
    {
        playGroundHitSound = null;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            ballMovement.IsGrounded = true;

        if (collision.collider.CompareTag("Hole"))
        {
            StartCoroutine(ballHandler.FinishCourse());
        }

        if (collision.collider.CompareTag("OoB"))
        {
            if (ballMovement.SpawningCoroutine == false)
            {
                StartCoroutine(ballMovement.ResetBall());
                ballMovement.SpawningCoroutine = true;
            }
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

        // Ground bounce particles
        if (collision.collider.CompareTag("Ground"))
        {
            if (collision.relativeVelocity.y >= 3)
            {
                // Only happens after a while, so the sound doesn't spam
                if (playGroundHitSound == null)
                    playGroundHitSound = StartCoroutine(PlayGroundHitSound());

                if (ballHandler.BezerraTempParticles)
                {
                    Vector3 dir = collision.contacts[0].normal * 500 - collision.contacts[0].point;

                    GameObject hitParticles = Instantiate(
                        ballHandler.PrefabLightBridgeCollision,
                        collision.contacts[0].point,
                        Quaternion.identity);

                    hitParticles.transform.LookAt(dir, Vector3.up);
                }

            }
        }

        // Invisible wall particles
        if (collision.collider.CompareTag("InvisWall"))
        {
            if (ballHandler.BezerraTempParticles)
            {
                Vector3 dir = collision.contacts[0].normal * 500 - collision.contacts[0].point;

                GameObject hitParticles = Instantiate(
                    ballHandler.PrefabLightBridgeCollision,
                    collision.contacts[0].point,
                    Quaternion.identity);

                hitParticles.transform.LookAt(dir, Vector3.up);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            ballMovement.IsGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OoB"))
        {
            if (ballMovement.SpawningCoroutine == false)
            {
                StartCoroutine(ballMovement.ResetBall());
                ballMovement.SpawningCoroutine = true;
            }
        }
    }


    /// <summary>
    /// Plays ground hit sound. Has a delay so the sound doesn't repeat too often.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayGroundHitSound()
    {
        ballSounds.PlaySound(BallSoundEnum.GroundhitSound);
        yield return new WaitForSeconds(2f);
        playGroundHitSound = null;
    }
}
