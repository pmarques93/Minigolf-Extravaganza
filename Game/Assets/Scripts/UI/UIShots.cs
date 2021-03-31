using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for handling shots UI.
/// </summary>
public class UIShots : MonoBehaviour
{
    // Components
    private BallHandler ballScore;

    // Text Components
    private TextMeshProUGUI uiShots;

    private void Awake()
    {
        ballScore = FindObjectOfType<BallHandler>();
        uiShots = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (ballScore != null) ballScore.Hit += UpdateUI;
    }

    private void OnDisable()
    {
        if (ballScore != null) ballScore.Hit -= UpdateUI;
    }

    /// <summary>
    /// Updates shots UI every time the player lands a shot.
    /// </summary>
    /// <param name="score">Score to register on ui.</param>
    private void UpdateUI(int score)
    {
        uiShots.text = score.ToString();
    }

    private void Update()
    {
        if (ballScore == null)
        {
            ballScore = FindObjectOfType<BallHandler>();
            if (ballScore != null) ballScore.Hit += UpdateUI;
        }
    }
}
