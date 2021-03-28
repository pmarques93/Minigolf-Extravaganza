using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using System;

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
    [SerializeField] private TextMeshProUGUI graphicsQuality;
    [SerializeField] private TextMeshProUGUI windowMode;
    [SerializeField] private TextMeshProUGUI screenResolution;

    // Components
    private Configuration configurationOptions;
    private EventSystem eventSys;

    private void Awake()
    {
        configurationOptions = FindObjectOfType<Configuration>();
        eventSys = FindObjectOfType<EventSystem>();
    }

    private void Start()
    {
        UpdateSliderValuesToMatchRealValues();  // Updates slider values
        UpdateTextValues();                     // Updates text values
    }

    private void Update()
    {
        Debug.Log(eventSys.currentSelectedGameObject);
    }

    /// <summary>
    /// Resets all values and updates interfaces.
    /// </summary>
    public void ResetControlsValues()
    {
        config.ResetControlsSettings();
        UpdateAllValuesWithInterfaces();
        UpdateSliderValuesToMatchRealValues();
    }

    /// <summary>
    /// Resets all values and updates interfaces.
    /// </summary>
    public void ResetGraphicsValues()
    {
        config.ResetGraphicsSettings();
        UpdateAllValuesWithInterfaces();
        UpdateTextValues();
        OnChangeQualityLevel();
        OnChangeScreenResolution();
        OnChangeWindowMode();
    }

    /// <summary>
    /// Resets all values and updates interfaces.
    /// </summary>
    public void ResetAudioValues()
    {
        config.ResetAudioSettings();
        UpdateAllValuesWithInterfaces();
        // UPDATE AUDIO VALUES
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

    public void LeftArrow(string option)
    {
        switch(option)
        {
            case "GraphicsQuality":
                if (config.GraphicsQuality - 1 >= config.MinGraphicsQuality)
                    config.GraphicsQuality--;
                else
                    config.GraphicsQuality = config.MaxGraphicsQuality;

                config.SaveSettings();
                UpdateAllValuesWithInterfaces();
                UpdateTextValues();
                OnChangeQualityLevel();
                break;
            case "WindowMode":
                if (config.WindowMode - 1 >= config.MinWindowMode)
                    config.WindowMode--;
                else
                    config.WindowMode = config.MaxWindowMode;

                config.SaveSettings();
                UpdateAllValuesWithInterfaces();
                UpdateTextValues();
                OnChangeScreenResolution();
                OnChangeWindowMode();
                break;
            case "ScreenResolution":
                if (config.ScreenResolution - 1 >= config.MinScreenResolution)
                    config.ScreenResolution--;
                else
                    config.ScreenResolution = config.MaxScreenResolution;

                config.SaveSettings();
                UpdateAllValuesWithInterfaces();
                UpdateTextValues();
                OnChangeScreenResolution();
                OnChangeWindowMode();
                break;
        }
    }

    public void RightArrow(string option)
    {
        switch (option)
        {
            case "GraphicsQuality":
                if (config.GraphicsQuality + 1 <= config.MaxGraphicsQuality)
                    config.GraphicsQuality++;
                else
                    config.GraphicsQuality = config.MinGraphicsQuality;

                config.SaveSettings();
                UpdateAllValuesWithInterfaces();
                UpdateTextValues();
                OnChangeQualityLevel();
                break;
            case "WindowMode":
                if (config.WindowMode + 1 <= config.MaxWindowMode)
                    config.WindowMode++;
                else
                    config.WindowMode = config.MinWindowMode;

                config.SaveSettings();
                UpdateAllValuesWithInterfaces();
                UpdateTextValues();
                OnChangeScreenResolution();
                OnChangeWindowMode();
                break;
            case "ScreenResolution":
                if (config.ScreenResolution + 1 <= config.MaxScreenResolution)
                    config.ScreenResolution++;
                else
                    config.ScreenResolution = config.MinScreenResolution;

                config.SaveSettings();
                UpdateAllValuesWithInterfaces();
                UpdateTextValues();   
                OnChangeScreenResolution();
                OnChangeWindowMode();
                break;
        }
    }

    /// <summary>
    /// Updates all text values in settings.
    /// </summary>
    private void UpdateTextValues()
    {
        switch (config.GraphicsQuality)
        {
            case 0:
                graphicsQuality.text = "Baixo";
                break;
            case 1:
                graphicsQuality.text = "Médio";
                break;
            case 2:
                graphicsQuality.text = "Alto";
                break;
        }
        switch (config.WindowMode)
        {
            case 0:
                windowMode.text = "Janela";
                break;
            case 1:
                windowMode.text = "Sem Bordas";
                break;
            case 2:
                windowMode.text = "Ecrã Inteiro";
                break;
        }
        switch (config.ScreenResolution)
        {
            case 0:
                screenResolution.text = "1280 x 720";
                break;
            case 1:
                screenResolution.text = "1600 × 900";
                break;
            case 2:
                screenResolution.text = "1920 x 1080";
                break;
        }
    }


    /// <summary>
    /// Selects the button before clicking the arrow after clicking the arrow.
    /// </summary>
    /// <param name="buttonToSelect"></param>
    public void SelectPreviousButton(GameObject buttonToSelect) => 
        StartCoroutine(SelectPreviousButtonCoroutine(buttonToSelect));

    /// <summary>
    /// Selects a button.
    /// </summary>
    /// <param name="buttonToSelect">Button to select.</param>
    /// <returns>WaitForFixedUpdate.</returns>
    private IEnumerator SelectPreviousButtonCoroutine(GameObject buttonToSelect)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        eventSys.SetSelectedGameObject(buttonToSelect);
    }

    protected virtual void OnChangeQualityLevel() => ChangeQualityLevel?.Invoke();

    protected virtual void OnChangeScreenResolution() => ChangeScreenResolution?.Invoke();

    protected virtual void OnChangeWindowMode() => ChangeWindowMode?.Invoke();

    /// <summary>
    /// Event registered on Configuration.
    /// </summary>
    public event Action ChangeQualityLevel;

    /// <summary>
    /// Event registered on Configuration.
    /// </summary>
    public event Action ChangeScreenResolution;

    /// <summary>
    /// Event registered on Configuration.
    /// </summary>
    public event Action ChangeWindowMode;
}
