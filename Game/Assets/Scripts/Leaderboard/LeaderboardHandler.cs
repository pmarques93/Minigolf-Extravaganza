using System.Collections;
using UnityEngine;

public class LeaderboardHandler : MonoBehaviour
{
	const string privateCode = "b8Tva4PRrUejuuiWTEyMiwg64T_s92zkigHTnOcr4OmA";
	const string publicCode = "6065d46d8f421366b05a8afb";
	const string webURL = "http://dreamlo.com/lb/";

	/// <summary>
	/// List with all highscores;
	/// </summary>
	public HighScore[] HighscoresList { get; private set; }

	/// <summary>
	/// Starts coroutine to add a new highscore.
	/// </summary>
	/// <param name="currentLevel">User name to add.</param>
	/// <param name="score">Score to add.</param>
    public void AddNewHighscore(string currentLevel, int score)
		=> StartCoroutine(UploadNewHighscore(currentLevel, score));

	/// <summary>
	/// Uploads a new high score.
	/// </summary>
	/// <param name="currentLevel"></param>
	/// <param name="score"></param>
	/// <returns></returns>
	private IEnumerator UploadNewHighscore(string currentLevel, int score)
	{
		// Deletes score for this user
		WWW www = 
			new WWW($"http://dreamlo.com/lb/b8Tva4PRrUejuuiWTEyMiwg64T_s92zkigHTnOcr4OmA/delete/{currentLevel}");

		// Uploads the new score
		www = 
			new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(currentLevel) + "/" + score);

		yield return www;

		if (string.IsNullOrEmpty(www.error))
		{
			DownloadHighScore();
			print("Uploaded Score");
		}
		else
		{
			print("Error uploading");
		}
	}

	/// <summary>
	/// Starts coroutine to load highscores.
	/// </summary>
	public void DownloadHighScore() =>
		StartCoroutine(DownloadHighScoreFromDatabase());

	/// <summary>
	/// Coroutine that loads all highscores from a database.
	/// </summary>
	/// <returns>Null.</returns>
	private IEnumerator DownloadHighScoreFromDatabase()
	{
		WWW www = new WWW(webURL + publicCode + "/pipe");
		yield return www;

		if (string.IsNullOrEmpty(www.error))
		{
			FormatHighScores(www.text);
		}
		else
		{
			print("Error downloading");
		}
	}

	/// <summary>
	/// Formats highscores in order to have highscore lists with names and scores.
	/// </summary>
	/// <param name="textStream">Textstream to format.</param>
	private void FormatHighScores(string textStream)
	{
		string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
		HighscoresList = new HighScore[entries.Length];

		for (int i = 0; i < entries.Length; i++)
		{
			string[] entryInfo = entries[i].Split(new char[] { '|' });
			string currentLevel = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			HighscoresList[i] = new HighScore(currentLevel, score);
		}
	}
}


