using UnityEngine;

/// <summary>
/// Class responsible for handling flag animations.
/// </summary>
public class FlagAnimation : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.ResetTrigger("FlagRise");
            anim.ResetTrigger("FlagToNormal");
            anim.SetTrigger("FlagRise");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.ResetTrigger("FlagToNormal");
            anim.ResetTrigger("FlagRise");
            anim.SetTrigger("FlagToNormal");
        }
    }
}
