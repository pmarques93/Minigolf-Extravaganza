using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// Class responsible for handling highscore in the end of each level.
/// </summary>
public class HighscoreHandler : MonoBehaviour
{
    // Components
    private LeaderboardHandler leaderboardHandler;
    private BallHandler ballHandler;
    private LevelPassed levelPassed;

    // Highscore
    public int CurrentHighscoreCurrentLevel { get; private set; }

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

        CurrentHighscoreCurrentLevel = 999;
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
        StartCoroutine(UpdateHighScoreCoroutine(numberOfPlays, leaderboardHandler.HighscoresList));

    /// <summary>
    /// If the level doesn't exist it creates a new highScore for that level.
    /// If the level exists, if the number of plays was better than the actual
    /// record, it overwrites that record with a new one.
    /// </summary>
    /// <param name="numberOfPlays">Number of plays to confirm.</param>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator UpdateHighScoreCoroutine(int numberOfPlays, HighScore[] highscore)
    { 
        yield return new WaitForFixedUpdate();

        // If the user had NO internet
        if (leaderboardHandler.HighscoresList == null)
        {
            // Score will be 999, meaning the player has no internet connection
            OnNoBeatHighScore();
        }
        // Else if the user had internet
        else
        {
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
                            OnBeatHighScore();
                            CurrentHighscoreCurrentLevel = numberOfPlays;
                        }
                        // If the player didn't beat the highscore
                        else
                        {
                            OnNoBeatHighScore();
                            CurrentHighscoreCurrentLevel =
                                highscore[i].Score;
                        }
                    }
                }
            }
            // If the record doesn't exist yet
            else
            {
                leaderboardHandler.AddNewHighscore(currentLevel, numberOfPlays);
                OnBeatHighScore();
                CurrentHighscoreCurrentLevel = numberOfPlays;
            }
        }
    }

    protected virtual void OnBeatHighScore() => BeatHighscore?.Invoke();

    /// <summary>
    /// Event registered on BlackSquareAnimationEvent.
    /// </summary>
    public event Action BeatHighscore;

    protected virtual void OnNoBeatHighScore() => NoBeatHighscore?.Invoke();

    /// <summary>
    /// Event registered on BlackSquareAnimationEvent.
    /// </summary>
    public event Action NoBeatHighscore;
}
