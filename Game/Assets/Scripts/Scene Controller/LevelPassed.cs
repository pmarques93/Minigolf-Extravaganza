using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling which levels the player passed.
/// </summary>
public class LevelPassed : MonoBehaviour
{
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
        if (ball != null) ball.Victory += UpdateLevelsPassed; 
    }

    private void OnDisable()
    {
        if (ball != null) ball.Victory -= UpdateLevelsPassed;
    }

    private void Update()
    {
        // Finds ball if it's null
        if (ball == null)
        {
            ball = FindObjectOfType<BallHandler>();
            if (ball != null) ball.Victory += UpdateLevelsPassed;
        }
    }

    /// <summary>
    /// Updates levels passed and updates playerprefs with the last level passed,
    /// so it can select that button in main menu.
    /// </summary>
    private void UpdateLevelsPassed()
    {
        // Adds current level to levels passed
        PlayerPrefs.SetInt(currentLevel.ToString(), 1);

        // Last level being passed to select it on main menu
        PlayerPrefs.SetString("LastLevelPassed", currentLevel.ToString());
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("LastLevelPassed");
    }
}
