using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Class responsible for handling game configurations.
/// </summary>
public class Configuration : MonoBehaviour
{
    [SerializeField] private ConfigurationScriptableObj config;
    [SerializeField] private UniversalRenderPipelineAsset[] qualitySettings;

    // Components
    private UISettings uiSettings;

    private void Awake()
    {
        uiSettings = FindObjectOfType<UISettings>();

        // Loads settings with the last values given by player every time a scene starts
        // If the values don't exists, it gives them their default value
        config.LoadSettings();

        // Updates values for every class that implements IUpdateConfigurations
        UpdateValuesForInterfaces();
    }

    private void Start()
    {
        UpdateQualitySettings();

        UpdateScreenResolution();

        UpdateWindowMode();
    }

    private void OnEnable()
    {
        uiSettings.ChangeQualityLevel += UpdateQualitySettings;
        uiSettings.ChangeScreenResolution += UpdateScreenResolution;
        uiSettings.ChangeWindowMode += UpdateWindowMode;
    }

    private void OnDisable()
    {
        uiSettings.ChangeQualityLevel -= UpdateQualitySettings;
        uiSettings.ChangeScreenResolution -= UpdateScreenResolution;
        uiSettings.ChangeWindowMode -= UpdateWindowMode;
    }

    /// <summary>
    /// Updates values for every class that implements IUpdateConfigurations.
    /// </summary>
    public void UpdateValuesForInterfaces()
    {
        SceneController sceneController = FindObjectOfType<SceneController>();
        GameObject[] rootGameObjects = sceneController.CurrentScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            IUpdateConfigurations[] childrenInterfaces =
                rootGameObject.GetComponentsInChildren<IUpdateConfigurations>();

            foreach (IUpdateConfigurations childInterface in childrenInterfaces)
            {
                childInterface.UpdateValues();
            }
        }
    }

    private void UpdateQualitySettings()
    {
        QualitySettings.SetQualityLevel(config.GraphicsQuality);
        QualitySettings.renderPipeline = qualitySettings[config.GraphicsQuality];
    }

    private void UpdateScreenResolution()
    {
        switch(config.ScreenResolution)
        {
            case 0:
                if (config.WindowMode == 1 || config.WindowMode == 2)
                    Screen.SetResolution(1280, 720, true);
                else
                    Screen.SetResolution(1280, 720, false);
                break;
            case 1:
                if (config.WindowMode == 1 || config.WindowMode == 2)
                    Screen.SetResolution(1600, 900, true);
                else
                    Screen.SetResolution(1600, 900, false);
                break;
            case 2:
                if (config.WindowMode == 1 || config.WindowMode == 2)
                    Screen.SetResolution(1920, 1080, true);
                else
                    Screen.SetResolution(1920, 1080, false);
                break;
        }
    }

    private void UpdateWindowMode()
    {
        FullScreenMode windowMode = default;
        switch(config.WindowMode)
        {
            case 0:
                windowMode = FullScreenMode.Windowed;
                break;
            case 1:
                windowMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                windowMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }
        Screen.fullScreenMode = windowMode;
    }
}
