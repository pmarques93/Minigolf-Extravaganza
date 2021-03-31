using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling which levels the player passed.
/// </summary>
public class LevelPassed : MonoBehaviour
{
    [Header("Which level is this")]
    [SerializeField] private LevelEnum currentLevel;
    public LevelEnum CurrentLevel => currentLevel;

    // Components
    private BallHandler ball;

    private void Awake()
    {
        ball = FindObjectOfType<BallHandler>();
    }

    private void OnEnable()
    {
        if (ball != null) ball.VictoryWithPlays += UpdateLevelsPassed; 
    }

    private void OnDisable()
    {
        if (ball != null) ball.VictoryWithPlays -= UpdateLevelsPassed;
    }

    private void Update()
    {
        // Finds ball if it's null
        if (ball == null)
        {
            ball = FindObjectOfType<BallHandler>();
            if (ball != null) ball.VictoryWithPlays += UpdateLevelsPassed;
        }
    }

    /// <summary>
    /// Updates levels passed and updates playerprefs with the last level passed,
    /// so it can select that button in main menu.
    /// </summary>
    private void UpdateLevelsPassed(int numberOfPlays)
    {
        // Last level being passed to select it on main menu
        PlayerPrefs.SetString("LastLevelPassed", currentLevel.ToString());

        // Adds current level to levels passed
        PlayerPrefs.SetInt(currentLevel.ToString() + "passedLevels", 1);

        // If the player has no highScore yet
        if (PlayerPrefs.HasKey(currentLevel.ToString() + "highScore") == false)
        {
            // Creates highscore
            PlayerPrefs.SetInt(currentLevel.ToString() + "highScore", numberOfPlays);
        }
        // Else if the player has a highScore
        else
        {
            // If number of plays was less than that highscore (which is good)
            if (numberOfPlays < PlayerPrefs.GetInt(currentLevel.ToString() + "highScore"))
            {
                // Sets new highScore
                PlayerPrefs.SetInt(currentLevel.ToString() + "highScore", numberOfPlays);
            }
            else { }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("LastLevelPassed");
    }
}
