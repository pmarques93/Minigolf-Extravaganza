using UnityEngine;

/// <summary>
/// Class responsible for controlling animator speed of an object.
/// </summary>
public class ObjectWithAnimator : MonoBehaviour, IUpdateConfigurations
{
    [SerializeField] private ConfigurationScriptableObj config;
    private Animator anim;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void UpdateValues()
    {
        anim.speed = config.WorldObstaclesSpeed;
    }

}
