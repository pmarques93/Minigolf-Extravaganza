using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Class responsible for handling audio configuration.
/// </summary>
public class AudioController : MonoBehaviour, IUpdateConfigurations
{
    [Tooltip("Time of transition to current sound values after loading a scene.")]
    [SerializeField] private float timeForTransition;

    // Components
    [SerializeField] private AudioMixer masterVolume;
    [SerializeField] private ConfigurationScriptableObj options;

    // Audio variables
    private float master;
    private float ambience;
    private float sfx;

    private void Awake()
    {
        masterVolume.SetFloat("masterVolume", options.MinMasterVolume);
        masterVolume.SetFloat("ambienceVolume", options.MinAmbienceVolume);
        masterVolume.SetFloat("sfxVolume", options.MinSfxVolume);
    }

    /// <summary>
    /// Only happens after configuration's awake (after loading all values)
    /// </summary>
    private IEnumerator Start()
    {
        YieldInstruction wfu = new WaitForFixedUpdate();

        // Current volumes
        master = options.MinMasterVolume;
        ambience = options.MinAmbienceVolume;
        sfx = options.MinSfxVolume;

        float timePassed = 0;
        // Scales volumes until they reach the current volume
        while (timePassed < timeForTransition)
        {
            // Lerps volume
            master = Mathf.Lerp(master, options.MasterVolume, timePassed / timeForTransition);
            ambience = Mathf.Lerp(ambience, options.AmbienceVolume, timePassed / timeForTransition);
            sfx = Mathf.Lerp(sfx, options.SfxVolume, timePassed / timeForTransition);

            // Updates current volumes for audio mixers
            masterVolume.SetFloat("masterVolume", master);
            masterVolume.SetFloat("ambienceVolume", ambience);
            masterVolume.SetFloat("sfxVolume", sfx);

            // Increments timePassed with current time
            timePassed = Time.time;
            yield return wfu;
        }
    }

    /// <summary>
    /// Updates values.
    /// </summary>
    public void UpdateValues()
    {
        masterVolume.SetFloat("masterVolume", options.MasterVolume);
        masterVolume.SetFloat("ambienceVolume", options.AmbienceVolume);
        masterVolume.SetFloat("sfxVolume", options.SfxVolume);
    }
}
