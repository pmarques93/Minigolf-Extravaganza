using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    // Values
    [SerializeField] private ConfigurationScriptableObj config;

    [SerializeField] private Slider ballRotation;
    [SerializeField] private Slider lineLength;
    [SerializeField] private Slider powerTime;
    [SerializeField] private Slider powerMultiplier;
    [SerializeField] private Slider freelookHorizontalRotation;
    [SerializeField] private Slider freelookVerticalRotation;

    // Components
    private Configuration configurationOptions;

    private void Awake()
    {
        configurationOptions = FindObjectOfType<Configuration>();
    }

    private void Start()
    {
        UpdateSliderValuesToMatchRealValues();
    }

    /// <summary>
    /// Resets all values and updates interfaces.
    /// </summary>
    public void ResetAllValues()
    {
        config.ResetSettings();
        UpdateAllValuesWithInterfaces();
        UpdateSliderValuesToMatchRealValues();
    }

    /// <summary>
    /// Updates all slider values to be the same as config values.
    /// </summary>
    private void UpdateSliderValuesToMatchRealValues()
    {
        ballRotation.value = config.LoadSetting<float>(SettingsEnum.BallRotationSpeed);
        ballRotation.minValue = config.MinRotationSpeed;
        ballRotation.maxValue = config.MaxRotationSpeed;
        lineLength.value = config.LoadSetting<float>(SettingsEnum.LineLength);
        lineLength.minValue = config.MinLineLength;
        lineLength.maxValue = config.MaxLineLength;
        powerTime.value = config.LoadSetting<float>(SettingsEnum.PowerTime);
        powerTime.minValue = config.MinPowerTime;
        powerTime.maxValue = config.MaxPowerTime;
        powerMultiplier.value = config.LoadSetting<float>(SettingsEnum.PowerMultiplier);
        powerMultiplier.minValue = config.MinPowerMultiplier;
        powerMultiplier.maxValue = config.MaxPowerMultiplier;
        freelookHorizontalRotation.value = config.LoadSetting<float>(SettingsEnum.FreeLookHorizontalRotation);
        freelookHorizontalRotation.minValue = config.MinFreelookHorizontalRotation;
        freelookHorizontalRotation.maxValue = config.MaxFreelookHorizontalRotation;
        freelookVerticalRotation.value = config.LoadSetting<float>(SettingsEnum.FreeLookVerticalRotation);
        freelookVerticalRotation.minValue = config.MinFreelookVerticalRotation;
        freelookVerticalRotation.maxValue = config.MaxFreelookVerticalRotation;

        config.SaveSettings();
    }

    /// <summary>
    /// Updates values for all interfaces that implement IUpdateConfiguration.
    /// </summary>
    public void UpdateAllValuesWithInterfaces() => 
        configurationOptions.UpdateValuesForInterfaces();

    public void BallRotationValue(float value)
    {
        config.RotationSpeed = value;
        config.SaveSettings();
    }

    public void LineLengthValue(float value)
    {
        config.LineLength = value;
        config.SaveSettings();
    }

    public void PowerTimeValue(float value)
    {
        config.PowerTime = value;
        config.SaveSettings();
    }

    public void PowerMultiplierValue(float value)
    {
        config.PowerMultiplier = value;
        config.SaveSettings();
    }

    public void FreeLookHorizontalRotationValue(float value)
    {
        config.FreelookHorizontalRotation = value;
        config.SaveSettings();
        UpdateAllValuesWithInterfaces();
    }

    public void FreeLookVerticalRotationValue(float value)
    {
        config.FreelookVerticalRotation = value;
        config.SaveSettings();
        UpdateAllValuesWithInterfaces();
    }
}
