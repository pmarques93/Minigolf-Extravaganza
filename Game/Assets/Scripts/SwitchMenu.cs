using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenu : MonoBehaviour
{
    [SerializeField] GameObject menuToClose;
    [SerializeField] GameObject menuToOpen;

    public void SwitchMenus()
    {
        if (menuToClose != null && menuToOpen != null)
        {
            menuToClose.SetActive(false);
            menuToOpen.SetActive(true);
        }
            

        
    }
}
