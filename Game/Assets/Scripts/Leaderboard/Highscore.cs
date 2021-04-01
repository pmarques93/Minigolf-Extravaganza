/// <summary>
/// Struct for highscores.
/// </summary>
public struct HighScore
{
	public string CurrentLevel { get; }
	public int Score { get; }

	public HighScore(string _currentLevel, int _score)
	{
		CurrentLevel = _currentLevel;
		Score = _score;
	}
}
