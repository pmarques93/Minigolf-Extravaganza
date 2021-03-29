using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

public class CourseManager : MonoBehaviour
{
    [SerializeField]
    private Transform startPosition;

    [SerializeField]
    private Transform endPosition;

    [SerializeField]
    private Ball BallPrefab;

    [SerializeField]
    private CinemachineFreeLook cam;

    Player playerInput;

    CourseCanvas courseCanvas;

    public float maxStoppedTime = 1.5f;

    private GameManager gameManager;

    private Ball[] balls;

    private int current;

    private int finishedBalls;

    /////////////////////////////////////////private InputMaster controls;

    private void Awake()
    {
        playerInput = GetComponentInChildren<Player>();
        courseCanvas = GetComponentInChildren<CourseCanvas>();

        /////////////////////////////////controls = new InputMaster();
        ////////////////////////////////controls.Player.SwitchCamera.performed += _ => SwitchCam();
        //////////////////////////////////controls.Player.FastForward.performed += _ => SwitchSpeed();
        #if UNITY_EDITOR
            /////////////////////////////controls.Debug.GoToHole.performed += _ => GoToEnd();
        #endif
    }

    void Start()
    {
        startPosition.GetComponentInChildren<MeshRenderer>().enabled = false;
        endPosition.GetComponentInChildren<MeshRenderer>().enabled = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        balls = new Ball[gameManager.PlayerList.Count];
        SetPlayers();
        current = 0;
        finishedBalls = 0;

        StartTurn();
    }

    private void Update()
    {
        courseCanvas.SetPuttsCounter(balls[current].Putts);
    }

    void GoToEnd()
    {
        balls[current].GoToPos(endPosition);
    }

    private void SwitchSpeed()
    {
        if (Time.timeScale == 1.0f)
            Time.timeScale = 2.5f;
        else
            Time.timeScale = 1.0f;
    }

    private void SwitchCam()
    {
        cam.enabled = !cam.enabled;
    }

    private void SetPlayers()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i] = (Instantiate(BallPrefab));
            balls[i].name = "Ball" + i;
            balls[i].SetupBall(startPosition, gameManager.PlayerList[i].color);
            startPosition.position += new Vector3(0.01f, 0, 0);
        }  
    }

    private void StartTurn()
    {
        balls[current].gameObject.SetActive(true);
        playerInput.setBall(balls[current]);
        balls[current].StartTurn();

        GameManager.PlayerStats p = gameManager.PlayerList[current];
        courseCanvas.SetPlayerName(p.name, p.color);
        

        cam.LookAt = balls[current].transform;
        cam.Follow = balls[current].transform;
    }

    public void EndTurn()
    {
        do
        {
            nextBall();
        } while (balls[current].FinishedCourse);
        
        StartTurn();
    }

    private void nextBall()
    {
        current++;
        if (current >= balls.Length)
            current = 0;
    }

    public void SaveScore(int putts)
    {
        finishedBalls++;
        gameManager.StorePutts(current, putts);
        

        if (finishedBalls == balls.Length)
        {
            gameManager.nextLevel();
        }
        else
        {
            EndTurn();
        }    
    }

    private void OnEnable()
    {
        /////////////////////////////////controls.Enable();
    }

    private void OnDisable()
    {
        //////////////////////////////////controls.Disable();
    }

}
