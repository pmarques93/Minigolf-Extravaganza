using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GraphicsMenu : Menu
{
    [SerializeField] TMP_Dropdown dropdownResolution = default;
    [SerializeField] TMP_Dropdown dropdownFullscreen = default;

    //-------------------------------------------------------------------------

    private Resolution[] resolutions;

    //-------------------------------------------------------------------------

    private void Awake()
    {
        SetFullscreenDropdown();
        SetResolutionsDropdown();
    }

    private void SetFullscreenDropdown()
    {
        int currentModeIndex = 0;
        FullScreenMode currentMode = Screen.fullScreenMode;
        switch (currentMode)
        {
            case FullScreenMode.FullScreenWindow:
                currentModeIndex = 0;
                break;

            case FullScreenMode.ExclusiveFullScreen:
                currentModeIndex = 1;
                break;

            case FullScreenMode.Windowed:
                currentModeIndex = 2;
                break;
        }

        dropdownFullscreen.value = currentModeIndex;
        dropdownFullscreen.RefreshShownValue();
    }

    private void SetResolutionsDropdown()
    {
        int currentResolutionIndex = 0;
        resolutions = Screen.resolutions.Select(resolution => new Resolution
        {
            width = resolution.width,
            height = resolution.height
        }).Distinct().ToArray();

        dropdownResolution.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        foreach (Resolution r in resolutions)
        {
            string aux = r.width + " x " + r.height;
            resolutionOptions.Add(aux);

            if (r.width == PlayerPrefs.GetInt("ResW", 1280) &&
                r.height == PlayerPrefs.GetInt("ResH", 720))
            {
                currentResolutionIndex = resolutionOptions.Count - 1;
            }
        }

        dropdownResolution.AddOptions(resolutionOptions);
        dropdownResolution.value = currentResolutionIndex;
        dropdownResolution.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];

        PlayerPrefs.SetInt("ResW", res.width);
        PlayerPrefs.SetInt("ResH", res.height);

        Screen.SetResolution(PlayerPrefs.GetInt("ResW"),
            PlayerPrefs.GetInt("ResH"), Screen.fullScreenMode);
    }

    public void SetFullScreen(int FullScreenIndex)
    {
        FullScreenMode mode = Screen.fullScreenMode;
        switch (FullScreenIndex)
        {
            case 0:
                mode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                mode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                mode = FullScreenMode.Windowed;
                break;
        }

        Screen.fullScreenMode = mode;
    }
}
