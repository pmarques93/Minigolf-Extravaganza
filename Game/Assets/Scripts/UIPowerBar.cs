using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for updating UI power bar.
/// </summary>
public class UIPowerBar : MonoBehaviour
{
    // Components
    private BallHandler ball;
    private Image panel; 

    private void Awake()
    {
        ball = FindObjectOfType<BallHandler>();
        panel = GetComponent<Image>();
    }

    /// <summary>
    /// Updates the power panel's fill amount.
    /// </summary>
    private void Update()
    {
        if (ball == null) ball = FindObjectOfType<BallHandler>();

        // If player is preparing a shot, the fill bar updates with the power
        if (ball != null)
        {
            if (ball.PreparingShot)
            {
                panel.fillAmount = ball.Power;
            }
            else
            {
                panel.fillAmount = 0;
            }
        }
    }
}
