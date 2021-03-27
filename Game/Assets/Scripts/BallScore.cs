using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Class responsible for handling ball score.
/// </summary>
public class BallScore : MonoBehaviour
{
    // Components
    private BallHandler ball;

    // Score variables
    private byte score;

    private void Awake()
    {
        ball = GetComponentInChildren<BallHandler>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        score = 0;
        OnScore(score);
    }

    private void OnEnable()
    {
        ball.ShotHit += UpdateScore;
    }

    private void OnDisable()
    {
        ball.ShotHit -= UpdateScore;
    }

    /// <summary>
    /// Every time the player lands a shot, the score updates.
    /// </summary>
    private void UpdateScore()
    {
        score++;
        OnScore(score);
    }

    protected virtual void OnScore(byte score) => Score?.Invoke(score);

    /// <summary>
    /// Event registered on UIShots;
    /// </summary>
    public event Action<byte> Score;
}
