using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] GameObject worldPanelParent;
    [SerializeField] GameObject panelLevels;

    public void SelectWorld()
    {
        foreach (Menu worldPanel in worldPanelParent.GetComponentsInChildren<Menu>())
        {
            worldPanel.gameObject.SetActive(false);
        }

        panelLevels.SetActive(true);
    }
}
