using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for handling shots UI.
/// </summary>
public class UIShots : MonoBehaviour
{
    // Components
    private BallShot ballShot;

    // Text Components
    private TextMeshProUGUI uiShots;

    private void Awake()
    {
        ballShot = FindObjectOfType<BallShot>();
        uiShots = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (ballShot != null) ballShot.Hit += UpdateUI;
    }

    private void OnDisable()
    {
        if (ballShot != null) ballShot.Hit -= UpdateUI;
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
        if (ballShot == null)
        {
            ballShot = FindObjectOfType<BallShot>();
            if (ballShot != null) ballShot.Hit += UpdateUI;
        }
    }
}
