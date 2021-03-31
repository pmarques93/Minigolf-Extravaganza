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
        }
    }
}
