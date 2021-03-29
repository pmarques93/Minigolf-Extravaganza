using UnityEngine;
using UnityEngine.EventSystems;

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

            switch (PlayerPrefs.GetString("LastLevelPassed"))
            {
                case "Flatland1":
                    allWorlds[0].SetActive(true);
                    eventSys.SetSelectedGameObject(allLevels[0]);
                    break;
                case "Flatland2":
                    allWorlds[0].SetActive(true);
                    eventSys.SetSelectedGameObject(allLevels[1]);
                    break;
                case "Flatland3":
                    allWorlds[0].SetActive(true);
                    eventSys.SetSelectedGameObject(allLevels[2]);
                    break;
                case "Flatland4":
                    allWorlds[0].SetActive(true);
                    eventSys.SetSelectedGameObject(allLevels[3]);
                    break;
            }
        }
    }
}
