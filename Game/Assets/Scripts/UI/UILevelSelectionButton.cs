using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILevelSelectionButton : MonoBehaviour
{
    // Components
    private Button myButton;
    [SerializeField] private TextMeshProUGUI blockedText;
    [SerializeField] private LevelEnum levelRequired;

    private void Awake()
    {
        myButton = GetComponent<Button>();

        // Player didn't pass this level yet
        if (PlayerPrefs.GetInt(levelRequired.ToString() + "passedLevels") == 0)
        {
            myButton.enabled = false;
            blockedText.gameObject.SetActive(true);
        }
        // Else if the player passed this level
        else
        {
            myButton.enabled = true;
            blockedText.gameObject.SetActive(false);
        }
    }
}
