using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;



    private bool isPaused;
    private float gameSpeed;
    /////////////////////////////private InputMaster controls;


    private void Awake()
    {
        //controls = new InputMaster();
        //////////////////////controls = humanInput.Controls;
        
        ////////////////////////////controls.Menu.Pause.performed += _ => UpdatePaused();
    }

    private void Start()
    {
        isPaused = false;
        gameSpeed = Time.timeScale;

        HideCursor();
    }

    private void UpdatePaused()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        if (!isPaused)
        {
            ///////////////////////////////////////controls.Player.Disable();
            isPaused = true;
            ShowCursor();
            gameSpeed = Time.timeScale;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            ////////////////////////////////////////controls.Player.Enable();
            isPaused = false;
            HideCursor();
            Time.timeScale = gameSpeed;



            foreach (Menu child in GetComponentsInChildren<Menu>())
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoMainMenu()
    {
        GameManager playerRecord = GameObject.Find("GameManager").GetComponent<GameManager>();
        Destroy(playerRecord.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


}
