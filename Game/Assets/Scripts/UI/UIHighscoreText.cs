using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// Class responsible for updating highscore's text on the final screen.
/// </summary>
public class UIHighscoreText : MonoBehaviour
{
    private HighscoreHandler highScore;
    private TextMeshProUGUI text;

    private void Awake()
    {
        highScore = FindObjectOfType<HighscoreHandler>();
        text = GetComponent<TextMeshProUGUI>();
    }

    private IEnumerator Start()
    {
        YieldInstruction wfs = new WaitForSeconds(1f);

        while (true)
        {
            // Means the player had no internet connection
            if (highScore.CurrentHighscoreCurrentLevel == 999)
            {
                text.text = "Sem conexão";
            }
            else
            {
                if (highScore.CurrentHighscoreCurrentLevel == 1)
                    text.text = 
                        highScore.CurrentHighscoreCurrentLevel.ToString() + " jogada";
                else
                    text.text = 
                        highScore.CurrentHighscoreCurrentLevel.ToString() + " jogadas";
            }

            yield return wfs;
        }
    }
}
