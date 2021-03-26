using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddPlayerMenu : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private TMP_InputField inputPlayerName;

    [SerializeField]
    private Button buttonAddPlayer;

    [SerializeField]
    private TextMeshProUGUI textPlayerList;

    [SerializeField]
    private Button buttonResetPlayers;

    [SerializeField]
    private int maxPlayers = 1;

    [SerializeField] Gamemode.Mode gameMode;

    //[SerializeField]
    //private Dropdown dropdownPlayerColor;

    [SerializeField]
    private Button buttonStart;

    private InputMaster controls;

    private NameGenerator nameGenerator;

    void Awake()
    {
        Time.timeScale = 1f;
        nameGenerator = new NameGenerator();
        textPlayerList.text = "";
        controls = new InputMaster();
        controls.Menu.Confirm.performed += _ => addPlayer();
    }

    public void addPlayer()
    {
        buttonStart.interactable = true;
        buttonResetPlayers.interactable = true;

        if (inputPlayerName.text.Equals(""))
            inputPlayerName.text = nameGenerator.GetRandomName();

        
        //playerRecord.AddPlayer(inputPlayerName.text, playerRecord.PlayerList.Count, Player.Type.Human);
        gameManager.AddPlayer(inputPlayerName.text, Player.Type.Human);
        addPlayerToList(inputPlayerName.text, gameManager.PlayerList[gameManager.PlayerList.Count - 1].color);

        inputPlayerName.text = "";

        if (gameManager.PlayerList.Count == maxPlayers)
            buttonAddPlayer.interactable = false;

    }

    public void ResetPlayers()
    {
        buttonResetPlayers.interactable = false;
        buttonStart.interactable = false;

        gameManager.Reset();
        nameGenerator.Reset();

        buttonAddPlayer.interactable = true;
        textPlayerList.text = "";
    }

    private void addPlayerToList(string pName, Color color)
    {
        textPlayerList.text += $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>" + pName + "</color>\n";
    }

    public void StartGame()
    {
        gameManager.nextLevel();
    }

    private void OnEnable()
    {
        gameManager.setGameMode(gameMode);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
