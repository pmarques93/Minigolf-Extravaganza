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

    [SerializeField] float defaultLineLength;
    [SerializeField] float minLineLength;
    [SerializeField] float maxLineLength;
    public float LineLength { get; set; }
    public float DefaultLineLength => defaultLineLength;
    public float MinLineLength => minLineLength;
    public float MaxLineLength => maxLineLength;

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

    [Header("Freelook Camera Rotation Settings")]
    [SerializeField] float defaultFreelookHorizontalRotation;
    [SerializeField] float minFreelookHorizontalRotation;
    [SerializeField] float maxFreelookHorizontalRotation;
    public float FreelookHorizontalRotation { get; set; }
    public float DefaultFreelookHorizontalRotation => defaultFreelookHorizontalRotation;
    public float MinFreelookHorizontalRotation => minFreelookHorizontalRotation;
    public float MaxFreelookHorizontalRotation => maxFreelookHorizontalRotation;

    [SerializeField] float defaultFreelookVerticalRotation;
    [SerializeField] float minFreelookVerticalRotation;
    [SerializeField] float maxFreelookVerticalRotation;
    public float FreelookVerticalRotation { get; set; }
    public float DefaultFreelookVerticalRotation => defaultFreelookVerticalRotation;
    public float MinFreelookVerticalRotation => minFreelookVerticalRotation;
    public float MaxFreelookVerticalRotation => maxFreelookVerticalRotation;

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

    /// <summary>
    /// Resets controls settings to default.
    /// </summary>
    public void ResetControlsSettings()
    {
        RotationSpeed = defaultRotationSpeed;
        LineLength = defaultLineLength;
        PowerTime = defaultPowerTime;
        PowerMultiplier = defaultPowerMultiplier;
        FreelookHorizontalRotation = defaultFreelookHorizontalRotation;
        FreelookVerticalRotation = defaultFreelookVerticalRotation;
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

    }

    /// <summary>
    /// Saves all settings.
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("RotationSpeed", RotationSpeed);
        PlayerPrefs.SetFloat("LineLength", LineLength);
        PlayerPrefs.SetFloat("PowerTime", PowerTime);
        PlayerPrefs.SetFloat("PowerMultiplier", PowerMultiplier);
        PlayerPrefs.SetFloat("FreelookHorizontalRotation", FreelookHorizontalRotation);
        PlayerPrefs.SetFloat("FreelookVerticalRotation", FreelookVerticalRotation);
        PlayerPrefs.SetInt("GraphicsQuality", GraphicsQuality);
        PlayerPrefs.SetInt("WindowMode", WindowMode);
        PlayerPrefs.SetInt("ScreenResolution", ScreenResolution);
    }

    /// <summary>
    /// Loads settings. If the settings don't exist, it gives them their default value.
    /// </summary>
    public void LoadSettings()
    {
        RotationSpeed = PlayerPrefs.GetFloat("RotationSpeed", defaultRotationSpeed);
        LineLength = PlayerPrefs.GetFloat("LineLength", defaultLineLength);
        PowerTime = PlayerPrefs.GetFloat("PowerTime", defaultPowerTime);
        PowerMultiplier = PlayerPrefs.GetFloat("PowerMultiplier", defaultPowerMultiplier);
        FreelookHorizontalRotation = PlayerPrefs.GetFloat("FreelookHorizontalRotation", defaultFreelookHorizontalRotation);
        FreelookVerticalRotation = PlayerPrefs.GetFloat("FreelookVerticalRotation", defaultFreelookVerticalRotation);
        GraphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", defaultGraphicsQuality);
        WindowMode = PlayerPrefs.GetInt("WindowMode", defaultWindowMode);
        ScreenResolution = PlayerPrefs.GetInt("ScreenResolution", defaultScreenResolution);
    }

    public T LoadSetting<T>(SettingsEnum setting)
    {
        T t = default;
        switch(setting)
        {
            case SettingsEnum.BallRotationSpeed:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("RotationSpeed", defaultRotationSpeed), typeof(T));
                break;
            case SettingsEnum.LineLength:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("LineLength", defaultLineLength), typeof(T));
                break;
            case SettingsEnum.PowerTime:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("PowerTime", defaultPowerTime), typeof(T));
                break;
            case SettingsEnum.PowerMultiplier:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("PowerMultiplier", defaultPowerMultiplier), typeof(T));
                break;
            case SettingsEnum.FreeLookHorizontalRotation:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("FreelookHorizontalRotation", defaultFreelookHorizontalRotation), typeof(T));
                break;
            case SettingsEnum.FreeLookVerticalRotation:
                t = (T)Convert.ChangeType(PlayerPrefs.GetFloat("FreelookVerticalRotation", defaultFreelookVerticalRotation), typeof(T));
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
        }
        return t;
    }
}
