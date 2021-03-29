using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField]
    private Button buttonMenu;

    [SerializeField]
    private Table table;


    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        ShowCursor();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        CreateTable();
    }

    private void CreateTable()
    {
        
        CreateTableHeader(gameManager.PlayerList[0].putts.Count);

        foreach (var player in gameManager.PlayerList)
        {
            
            CreateTableRow(player);
        }
    }

    private void CreateTableHeader(int columns)
    {
        string[] values = new string[columns+1];

        for (int i = 0; i <= values.Length - 1; i++)
        {
            values[i] = (i + 1).ToString();
        }

        values[values.Length - 1] = "T";

        table.CreateRow("Players", values);
    }

    private void CreateTableRow(GameManager.PlayerStats player)
    {
        int[] scores = player.putts.ToArray();
        string[] values = new string[scores.Length + 1];
        int total = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            values[i] = scores[i].ToString();
            total += scores[i];
        }

        values[values.Length - 1] = total.ToString();

        table.CreateRow(player.name, values, player.color);
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GoMainMenu()
    {
        Destroy(gameManager.gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
