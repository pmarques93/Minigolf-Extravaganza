using UnityEngine;

/// <summary>
/// Class responsible for controling game's timescale.
/// </summary>
public class GameTimeScale : MonoBehaviour, IUpdateConfigurations
{
    [SerializeField] private ConfigurationScriptableObj config;

    public float CurrentTimeScale { get; private set; }

    private void Awake()
    {
        CurrentTimeScale = config.GameSpeed;
        Time.timeScale = CurrentTimeScale;
    }

    /// <summary>
    /// Updates CurrentTimeScale. This value will be applied on PauseMenu -> UnpauseGame();
    /// </summary>
    public void UpdateValues()
    {
        CurrentTimeScale = config.GameSpeed;
    }
}
