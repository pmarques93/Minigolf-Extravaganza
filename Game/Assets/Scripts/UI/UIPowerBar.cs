using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Class responsible for updating UI power bar.
/// </summary>
public class UIPowerBar : MonoBehaviour
{
    // Components
    private BallHandler ball;
    private Image panel;

    private bool canCopyPower;

    private void Awake()
    {
        ball = FindObjectOfType<BallHandler>();
        panel = GetComponent<Image>();
    }

    private IEnumerator Start()
    {
        canCopyPower = false;

        float time = 1;
        while (time >= 0)
        {
            panel.fillAmount = time;
            time -= 0.2f * Time.fixedUnscaledDeltaTime;
            yield return null;
        }
        canCopyPower = true;
    }

    /// <summary>
    /// Updates the power panel's fill amount.
    /// </summary>
    private void Update()
    {
        if (ball == null) ball = FindObjectOfType<BallHandler>();

        // If player is preparing a shot, the fill bar updates with the power
        if (ball != null && canCopyPower)
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
