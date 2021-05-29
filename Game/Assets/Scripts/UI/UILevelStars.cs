using UnityEngine;

public class UILevelStars : MonoBehaviour
{
    [Header("Level of this button")]
    [SerializeField] private LevelEnum currentLevel;

    [Header("Score evaluation of this level")]
    [SerializeField] private int[] scoresEvaluation;

    [Header("Groups of filled and unfilled stars")]
    [SerializeField] private GameObject[] starGroups;

    private void Start()
    {
        // If the score exists, gives a value to currentHighScore, else it's 0
        int currentHighScore;
        if (PlayerPrefs.HasKey(currentLevel.ToString() + "highScore"))
            currentHighScore = PlayerPrefs.GetInt(currentLevel.ToString() + "highScore");
        else
            currentHighScore = 999;

        foreach (GameObject starGroup in starGroups)
            if (starGroup.activeSelf) starGroup.SetActive(false);

        // Good
        if (currentHighScore <= scoresEvaluation[0])
        {
            starGroups[3].SetActive(true);
        }
        // Neutral
        else if (currentHighScore <= scoresEvaluation[1])
        {
            starGroups[2].SetActive(true);
        }
        // Bad
        else if (currentHighScore <= scoresEvaluation[2])
        {
            starGroups[1].SetActive(true);
        }
        // Very bad or no record
        else
        {
            starGroups[0].SetActive(true);
        }
    }
}
