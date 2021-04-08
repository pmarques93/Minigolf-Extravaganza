using UnityEngine;

/// <summary>
/// Class responsible for handling ball's sounds.
/// </summary>
public class BallSounds : MonoBehaviour
{
    private AudioSource audioSource;

    // Sounds
    [Header("Sounds")]
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip groundHitSound;
    [SerializeField] private AudioClip confetiSound;
    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(BallSoundEnum ballSound)
    {
        audioSource.pitch = 1f;
        switch (ballSound)
        {
            case BallSoundEnum.HitSound:
                int randomSound = UnityEngine.Random.Range(0, hitSounds.Length);
                audioSource.PlayOneShot(hitSounds[randomSound]);
                break;
            case BallSoundEnum.ConfetiSound:
                audioSource.PlayOneShot(confetiSound);
                break;
            case BallSoundEnum.GroundhitSound:
                float randomPitch = UnityEngine.Random.Range(0.5f, 1f);
                audioSource.pitch = randomPitch;
                audioSource.PlayOneShot(groundHitSound);
                break;
        }
    }
}
