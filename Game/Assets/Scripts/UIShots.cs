using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for handling shots UI.
/// </summary>
public class UIShots : MonoBehaviour
{
    // Components
    private BallScore ballScore;

    // Text Components
    private TextMeshProUGUI uiShots;

    private void Awake()
    {
        ballScore = FindObjectOfType<BallScore>();
        uiShots = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (ballScore != null) ballScore.Score += UpdateUI;
    }

    private void OnDisable()
    {
        if (ballScore != null) ballScore.Score -= UpdateUI;
    }

    /// <summary>
    /// Updates shots UI every time the player lands a shot.
    /// </summary>
    /// <param name="score">Score to register on ui.</param>
    private void UpdateUI(byte score)
    {
        uiShots.text = score.ToString();
    }

    private void Update()
    {
        if (ballScore == null)
        {
            ballScore = FindObjectOfType<BallScore>();
            if (ballScore != null) ballScore.Score += UpdateUI;
        }
    }
}
