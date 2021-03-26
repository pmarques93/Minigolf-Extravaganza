using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isTest;

    public List<PlayerStats> PlayerList { get; private set; }

    public Color[] playerColor;

    private List<Color> avaiableColors;

    public int maxPlayers { get; set; }

    public static Gamemode.Mode currentMode { get; set; }
    // -----------------------------------------------------------------------

    private int remainingLevels;

    private void Start()
    {
        if (isTest)
        {
            AddPlayer("Tester", Player.Type.Human);
        }
    }

    public void AddPlayer(string name, int colorIndex, Player.Type type)
    {
        PlayerList.Add(new PlayerStats(name, playerColor[colorIndex], type));
    }

    public void AddPlayer(string name, Player.Type type)
    {
        int colorIndex;
        colorIndex = Random.Range(0, avaiableColors.Count - 1);
        PlayerList.Add(new PlayerStats(name, avaiableColors[colorIndex], type));
        avaiableColors.RemoveAt(colorIndex);
    }

    public void Reset()
    {
        avaiableColors = new List<Color>(playerColor);
        PlayerList = new List<PlayerStats>();
    }

    public void nextLevel()
    {
        remainingLevels--;

        if (remainingLevels >= 0)
            SceneManager.LoadScene("LevelSelect");
        else
            SceneManager.LoadScene("Scoreboard");
    }

    public void StorePutts(int player, int putts)
    {
        PlayerList[player].putts.Add(putts);
    }

    public string getName(int index)
    {
        return PlayerList[index].name;
    }

    public class PlayerStats
    {
        public string name;
        public Color color;
        public List<int> putts;
        public Player.Type type;

        public PlayerStats (string name, Color color, Player.Type type)
        {
            this.name = name;
            this.color = color;
            this.type = type;
            putts = new List<int>();
        }
    }

    public void setGameMode(Gamemode.Mode mode)
    {
        currentMode = mode;
        switch (mode)
        {
            case Gamemode.Mode.Training:
                remainingLevels = 1;
                break;

            case Gamemode.Mode.Versus:
                remainingLevels = 4;
                break;
        }
    }

    private void OnEnable()
    {
        avaiableColors = new List<Color>(playerColor);
        PlayerList = new List<PlayerStats>();

        DontDestroyOnLoad(gameObject);
    }
}
