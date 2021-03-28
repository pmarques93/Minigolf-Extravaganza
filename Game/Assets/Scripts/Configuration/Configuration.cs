using UnityEngine;

/// <summary>
/// Class responsible for handling game configurations.
/// </summary>
public class Configuration : MonoBehaviour
{
    [SerializeField] private ConfigurationScriptableObj config;

    private void Start()
    {
        // Loads settings with the last values given by player
        // If the values don't exists, it gives them their default value
        config.LoadSettings();

        // Updates values for every class that implements IUpdateConfigurations
        UpdateValuesForInterfaces();
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
}
