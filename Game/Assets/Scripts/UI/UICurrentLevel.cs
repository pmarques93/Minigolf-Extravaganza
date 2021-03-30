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
            case LevelEnum.Flatland1:
                currentLevelText.text = "Nível 1-1";
                break;
            case LevelEnum.Flatland2:
                currentLevelText.text = "Nível 1-2";
                break;
            case LevelEnum.Flatland3:
                currentLevelText.text = "Nível 1-3";
                break;
            case LevelEnum.Flatland4:
                currentLevelText.text = "Nível 1-4";
                break;
            case LevelEnum.Flatland5:
                currentLevelText.text = "Nível 1-5";
                break;
            case LevelEnum.Flatland6:
                currentLevelText.text = "Nível 1-6";
                break;
        }
    }
}
