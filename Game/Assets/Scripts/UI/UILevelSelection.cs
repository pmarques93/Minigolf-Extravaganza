using UnityEngine;

/// <summary>
/// Class responsible for handling level selection UI.
/// </summary>
public class UILevelSelection : MonoBehaviour
{
    private PlayerInputCustom input;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
    }

    /// <summary>
    /// Disables UI input module.
    /// </summary>
    public void SwitchControlsToDisableOnMenu() => 
        input.SwitchControlsToDisableOnMenu();
}
