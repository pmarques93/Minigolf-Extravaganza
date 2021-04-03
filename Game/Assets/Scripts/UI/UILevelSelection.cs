using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// Class responsible for handling level selection UI.
/// </summary>
public class UILevelSelection : MonoBehaviour
{
    [SerializeField] private GameObject splashScreen;
    [SerializeField] private GameObject[] allWorlds;
    [SerializeField] private GameObject[] allLevels;

    // Components
    private EventSystem eventSys;

    private void Awake()
    {
        eventSys = FindObjectOfType<EventSystem>();

        // If the player came from a level instead of starting the game
        if (PlayerPrefs.HasKey("LastLevelPassed"))
        {
            splashScreen.SetActive(false);

            // Converts level passed to an enum
            LevelEnum lastLevelPassedName;
            Enum.TryParse(PlayerPrefs.GetString("LastLevelPassed"), out lastLevelPassedName);

            switch (lastLevelPassedName)
            {
                case LevelEnum.Park1:
                    allWorlds[0].SetActive(true);
                    eventSys.SetSelectedGameObject(allLevels[0]);
                    break;
                case LevelEnum.Park2:
                    allWorlds[0].SetActive(true);
                    eventSys.SetSelectedGameObject(allLevels[1]);
                    break;
                case LevelEnum.Park3:
                    allWorlds[0].SetActive(true);
                    eventSys.SetSelectedGameObject(allLevels[2]);
                    break;
                case LevelEnum.Park4:
                    allWorlds[0].SetActive(true);
                    eventSys.SetSelectedGameObject(allLevels[3]);
                    break;
                case LevelEnum.Park5:
                    allWorlds[0].SetActive(true);
                    eventSys.SetSelectedGameObject(allLevels[4]);
                    break;
                case LevelEnum.Park6:
                    allWorlds[0].SetActive(true);
                    eventSys.SetSelectedGameObject(allLevels[5]);
                    break;
            }
        }
    }

    /// <summary>
    /// Called from UI main menu buttons.
    /// </summary>
    /// <param name="levelToLoad">Level to load.</param>
    public void LoadLevel(string levelToLoad)
    {
        if (Enum.TryParse(levelToLoad, out LevelEnum level))
            FindObjectOfType<SceneController>().LoadLevel(level);
        else
            throw new NotSupportedException();
    }
}
