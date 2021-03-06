using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// Class responsible for handling main menu UI and functionality.
/// </summary>
public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject initialSelectedGameobject;

    // Components
    private EventSystem eventSys;

    private void Awake()
    {
        Time.timeScale = 1f;
        eventSys = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        // Every time there is not an object selected, it selects resume button
        if (eventSys.currentSelectedGameObject == null)
        {
            eventSys.SetSelectedGameObject(initialSelectedGameobject);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
