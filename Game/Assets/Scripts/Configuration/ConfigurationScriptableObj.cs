using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Scriptable object responsible for keeping configuration settings.
/// </summary>
[CreateAssetMenu(fileName = "Configuration")]
public class ConfigurationScriptableObj : ScriptableObject
{
    [Header("Ball Settings")]
    [SerializeField] float defaultRotationSpeed;
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    public float RotationSpeed { get; set; }
    public float DefaultRotationSpeed => defaultRotationSpeed;
    public float MinRotationSpeed => minRotationSpeed;
    public float MaxRotationSpeed => maxRotationSpeed;

    [SerializeField] float defaultPowerTime;
    [SerializeField] float minPowerTime;
    [SerializeField] float maxPowerTime;
    public float PowerTime { get; set; }
    public float DefaultPowerTime => defaultPowerTime;
    public float MinPowerTime => minPowerTime;
    public float MaxPowerTime => maxPowerTime;

    [SerializeField] float defaultPowerMultiplier;
    [SerializeField] float minPowerMultiplier;
    [SerializeField] float maxPowerMultiplier;
    public float PowerMultiplier { get; set; }
    public float DefaultPowerMultiplier => defaultPowerMultiplier;
    public float MinPowerMultiplier => minPowerMultiplier;
    public float MaxPowerMultiplier => maxPowerMultiplier;

    [SerializeField] float defaultWorldObstaclesSpeed;
    [SerializeField] float minWorldObstaclesSpeed;
    [SerializeField] float maxWorldObstaclesSpeed;
    public float WorldObstaclesSpeed { get; set; }
    public float DefaultWorldObstaclesSpeed => defaultWorldObstaclesSpeed;
    public float MinWorldObstaclesSpeed => minWorldObstaclesSpeed;
    public float MaxWorldObstaclesSpeed => maxWorldObstaclesSpeed;

    [SerializeField] float defaultGameSpeed;
    [SerializeField] float minGameSpeed;
    [SerializeField] float maxGameSpeed;
    public float GameSpeed { get; set; }
    public float DefaultGameSpeed => defaultGameSpeed;
    public float MinGameSpeed => minGameSpeed;
    public float MaxGameSpeed => maxGameSpeed;

    [Header("Graphics Settings")]
    [SerializeField] int defaultGraphicsQuality;
    [SerializeField] int minGraphicsQuality;
    [SerializeField] int maxGraphicsQuality;
    public int GraphicsQuality { get; set; }
    public int DefaultGraphicsQuality => defaultGraphicsQuality;
    public int MinGraphicsQuality => minGraphicsQuality;
    public int MaxGraphicsQuality => maxGraphicsQuality;

    [SerializeField] int defaultWindowMode;
    [SerializeField] int minWindowMode;
    [SerializeField] int maxWindowMode;
    public int WindowMode { get; set; }
    public int DefaultWindowMode => defaultWindowMode;
    public int MinWindowMode => minWindowMode;
    public int MaxWindowMode => maxWindowMode;

    [SerializeField] int defaultScreenResolution;
    [SerializeField] int minScreenResolution;
    [SerializeField] int maxScreenResolution;
    public int ScreenResolution { get; set; }
    public int DefaultScreenResolution => defaultScreenResolution;
    public int MinScreenResolution => minScreenResolution;
    public int MaxScreenResolution => maxScreenResolution;

    [Header("Audio Settings")]
    [SerializeField] float defaultMasterVolume;
    [SerializeField] float minMasterVolume;
    [SerializeField] float maxMasterVolume;
    public float MasterVolume { get; set; }
    public float DefaultMasterVolume => defaultMasterVolume;
    public float MinMasterVolume => minMasterVolume;
    public float MaxMasterVolume => maxMasterVolume;

    [SerializeField] float defaultAmbienceVolume;
    [SerializeField] float minAmbienceVolume;
    [SerializeField] float maxAmbienceVolume;
    public float AmbienceVolume { get; set; }
    public float DefaultAmbienceVolume => defaultAmbienceVolume;
    public float MinAmbienceVolume => minAmbienceVolume;
    public float MaxAmbienceVolume => maxAmbienceVolume;

    [SerializeField] float defaultSfxVolume;
    [SerializeField] float minSfxVolume;
    [SerializeField] float maxSfxVolume;
    public float SfxVolume { get; set; }
    public float DefaultSfxVolume => defaultSfxVolume;
    public float MinSfxVolume => minSfxVolume;
    public float MaxSfxVolume => maxSfxVolume;

    /// <summary>
    /// Resets controls settings to default.
    /// </summary>
    public void ResetControlsSettings()
    {
        RotationSpeed = defaultRotationSpeed;
        PowerTime = defaultPowerTime;
        PowerMultiplier = defaultPowerMultiplier;
        WorldObstaclesSpeed = defaultWorldObstaclesSpeed;
        GameSpeed = defaultGameSpeed;
    }

    /// <summary>
    /// Resets graphics settings to default.
    /// </summary>
    public void ResetGraphicsSettings()
    {
        GraphicsQuality = defaultGraphicsQuality;
        WindowMode = defaultWindowMode;
        ScreenResolution = defaultScreenResolution;
    }

    /// <summary>
    /// Resets audio settings to default.
    /// </summary>
    public void ResetAudioSettings()
    {
        MasterVolume = defaultMasterVolume;
        AmbienceVolume = defaultAmbienceVolume;
        SfxVolume = defaultSfxVolume;
    }

    /// <summary>
    /// Saves all settings.
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("RotationSpeed", RotationSpeed);
        PlayerPrefs.SetFloat("PowerTime", PowerTime);
        PlayerPrefs.SetFloat("PowerMultiplier", PowerMultiplier);
        PlayerPrefs.SetFloat("WorldObstaclesSpeed", WorldObstaclesSpeed);
        PlayerPrefs.SetFloat("GameSpeed", GameSpeed);
        PlayerPrefs.SetInt("GraphicsQuality", GraphicsQuality);
        PlayerPrefs.SetInt("WindowMode", WindowMode);
        PlayerPrefs.SetInt("ScreenResolution", ScreenResolution);
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat("AmbienceVolume", AmbienceVolume);
        PlayerPrefs.SetFloat("SfxVolume", SfxVolume);
    }

    /// <summary>
    /// Loads settings. If the settings don't exist, it gives them their default value.
    /// </summary>
    public void LoadSettings()
    {
        RotationSpeed = PlayerPrefs.GetFloat("RotationSpeed", defaultRotationSpeed);
        PowerTime = PlayerPrefs.GetFloat("PowerTime", defaultPowerTime);
        PowerMultiplier = PlayerPrefs.GetFloat("PowerMultiplier", defaultPowerMultiplier);
        WorldObstaclesSpeed = PlayerPrefs.GetFloat("WorldObstaclesSpeed", defaultWorldObstaclesSpeed);
        GameSpeed = PlayerPrefs.GetFloat("GameSpeed", defaultGameSpeed);
        GraphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", defaultGraphicsQuality);
        WindowMode = PlayerPrefs.GetInt("WindowMode", defaultWindowMode);
        ScreenResolution = PlayerPrefs.GetInt("ScreenResolution", defaultScreenResolution);
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume);
        AmbienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", defaultAmbienceVolume);
        SfxVolume = PlayerPrefs.GetFloat("SfxVolume", defaultSfxVolume);
    }

    /// <summary>
    /// Loads a determined setting.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    /// <param name="setting">Setting to load.</param>
    /// <returns>Type of value.</returns>
    public T LoadSetting<T>(SettingsEnum setting)
    {
        T t = default;
        switch(setting)
        {
            case SettingsEnum.BallRotationSpeed:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("RotationSpeed", defaultRotationSpeed), typeof(T));
                break;
            case SettingsEnum.PowerTime:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("PowerTime", defaultPowerTime), typeof(T));
                break;
            case SettingsEnum.PowerMultiplier:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("PowerMultiplier", defaultPowerMultiplier), typeof(T));
                break;
            case SettingsEnum.WorldObstaclesSpeed:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("WorldObstaclesSpeed", defaultWorldObstaclesSpeed), typeof(T));
                break;
            case SettingsEnum.GameSpeed:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("GameSpeed", defaultGameSpeed), typeof(T));
                break;
            case SettingsEnum.GraphicsQuality:
                t = (T)Convert.ChangeType(PlayerPrefs.GetInt("GraphicsQuality", defaultGraphicsQuality), typeof(T));
                break;
            case SettingsEnum.WindowMode:
                t = (T)Convert.ChangeType(PlayerPrefs.GetInt("WindowMode", defaultWindowMode), typeof(T));
                break;
            case SettingsEnum.ScreenResolution:
                t = (T)Convert.ChangeType(PlayerPrefs.GetInt("ScreenResolution", defaultScreenResolution), typeof(T));
                break;
            case SettingsEnum.MasterVolume:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume), typeof(T));
                break;
            case SettingsEnum.AmbienceVolume:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("AmbienceVolume", defaultAmbienceVolume), typeof(T));
                break;
            case SettingsEnum.SfxVolume:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("SfxVolume", defaultSfxVolume), typeof(T));
                break;
        }
        return t;
    }
}
