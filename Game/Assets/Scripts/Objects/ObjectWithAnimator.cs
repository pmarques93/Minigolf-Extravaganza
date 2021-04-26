using UnityEngine;
using System.Collections;

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
        StartCoroutine(UpdateValuesCoroutine());
    }

    private IEnumerator UpdateValuesCoroutine()
    {
        yield return new WaitForFixedUpdate();
        anim.speed = config.WorldObstaclesSpeed;
    }
}
