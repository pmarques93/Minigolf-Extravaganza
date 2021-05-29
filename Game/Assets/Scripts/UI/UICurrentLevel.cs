using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for updating current level UI.
/// </summary>
public class UICurrentLevel : MonoBehaviour
{
    // Components
    private LevelPassed currentScene;
    private TextMeshProUGUI currentLevelText;

    private void Awake()
    {
        currentScene = FindObjectOfType<LevelPassed>();
        currentLevelText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        switch (currentScene.CurrentLevel)
        {
            case LevelEnum.Park1:
                currentLevelText.text = "N�vel 1-1";
                break;
            case LevelEnum.Park2:
                currentLevelText.text = "N�vel 1-2";
                break;
            case LevelEnum.Park3:
                currentLevelText.text = "N�vel 1-3";
                break;
            case LevelEnum.Park4:
                currentLevelText.text = "N�vel 1-4";
                break;
            case LevelEnum.Park5:
                currentLevelText.text = "N�vel 1-5";
                break;
            case LevelEnum.Park6:
                currentLevelText.text = "N�vel 1-6";
                break;
            case LevelEnum.SciFi1:
                currentLevelText.text = "N�vel 2-1";
                break;
            case LevelEnum.SciFi2:
                currentLevelText.text = "N�vel 2-2";
                break;
            case LevelEnum.SciFi3:
                currentLevelText.text = "N�vel 2-3";
                break;
            case LevelEnum.SciFi4:
                currentLevelText.text = "N�vel 2-4";
                break;
            case LevelEnum.SciFi5:
                currentLevelText.text = "N�vel 2-5";
                break;
            case LevelEnum.SciFi6:
                currentLevelText.text = "N�vel 2-6";
                break;
            case LevelEnum.Medieval1:
                currentLevelText.text = "N�vel 3-1";
                break;
            case LevelEnum.Medieval2:
                currentLevelText.text = "N�vel 3-2";
                break;
            case LevelEnum.Medieval3:
                currentLevelText.text = "N�vel 3-3";
                break;
            case LevelEnum.Medieval4:
                currentLevelText.text = "N�vel 3-4";
                break;
            case LevelEnum.Medieval5:
                currentLevelText.text = "N�vel 3-5";
                break;
            case LevelEnum.Medieval6:
                currentLevelText.text = "N�vel 3-6";
                break;
        }
    }
}
