using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Class responsible for updating UI power bar.
/// </summary>
public class UIPowerBar : MonoBehaviour
{
    // Components
    private BallShot ballShot;
    private Image panel;

    private bool canCopyPower;

    private void Awake()
    {
        ballShot = FindObjectOfType<BallShot>();
        panel = GetComponent<Image>();

        canCopyPower = false;
    }

    private IEnumerator Start()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        canCopyPower = false;

        float time = 1;
        while (time >= 0)
        {
            panel.fillAmount = time;
            time -= 0.3f * Time.fixedDeltaTime;
            yield return wffu;
        }
        canCopyPower = true;
    }

    /// <summary>
    /// Updates the power panel's fill amount.
    /// </summary>
    private void Update()
    {
        if (ballShot == null) ballShot = FindObjectOfType<BallShot>();

        // If player is preparing a shot, the fill bar updates with the power
        if (ballShot != null && canCopyPower)
        {
            if (ballShot.PreparingShot)
            {
                panel.fillAmount = ballShot.Power;
            }
            else
            {
                panel.fillAmount = 0;
            }
        }
    }
}
