using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling highscore in the end of each level.
/// </summary>
public class HighscoreHandler : MonoBehaviour
{
    // Components
    private LeaderboardHandler leaderboardHandler;
    private BallHandler ballHandler;
    private LevelPassed levelPassed;

    private void Awake()
    {
        leaderboardHandler = GetComponent<LeaderboardHandler>();
        ballHandler = FindObjectOfType<BallHandler>();
        levelPassed = FindObjectOfType<LevelPassed>();
    }

    /// <summary>
    /// Updates highscores.
    /// </summary>
    private void Start()
    {
        // Updates highscores.
        leaderboardHandler.DownloadHighScore();
    }

    #region Registering to events
    private void OnEnable()
    {
        if (ballHandler != null) ballHandler.VictoryWithPlays += UpdateHighScore;
    }

    private void OnDisable()
    {
        if (ballHandler != null) ballHandler.VictoryWithPlays -= UpdateHighScore;
    }

    private void Update()
    {
        if (ballHandler == null)
        {
            ballHandler = FindObjectOfType<BallHandler>();
            if (ballHandler != null) ballHandler.VictoryWithPlays += UpdateHighScore;
        }
    }
    #endregion

    /// <summary>
    /// Starts coroutine to update highscore.
    /// </summary>
    /// <param name="numberOfPlays"></param>
    private void UpdateHighScore(int numberOfPlays) => 
        StartCoroutine(UpdateHighScoreCoroutine(numberOfPlays));

    /// <summary>
    /// If the level doesn't exist it creates a new highScore for that level.
    /// If the level exists, if the number of plays was better than the actual
    /// record, it overwrites that record with a new one.
    /// </summary>
    /// <param name="numberOfPlays">Number of plays to confirm.</param>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator UpdateHighScoreCoroutine(int numberOfPlays)
    {
        yield return new WaitForFixedUpdate();

        // Loops until it finds highscore list
        HighScore[] highscore = null;
        while (highscore == null)
        {
            highscore = leaderboardHandler.HighscoresList;
            yield return null;
        }
        
        string currentLevel = levelPassed.CurrentLevel.ToString();

        // Checks if this record already exists
        bool currentLevelExists = false;
        for (int i = 0; i < highscore.Length; i++)
        {
            if (highscore[i].CurrentLevel == currentLevel)
            {
                currentLevelExists = true;
            }
        }

        // If the record exists
        if (currentLevelExists)
        {
            // For all highscores
            for (int i = 0; i < highscore.Length; i++)
            {
                // If it's this current level's highscore
                if (highscore[i].CurrentLevel == currentLevel)
                {
                    // If the player had less plays than the current highscore
                    if (numberOfPlays < highscore[i].Score)
                    {
                        leaderboardHandler.AddNewHighscore(currentLevel, numberOfPlays);
                    }
                }
            }
        }
        // If the record doesn't exist yet
        else
        {
            leaderboardHandler.AddNewHighscore(currentLevel, numberOfPlays);
        }
    }
}
