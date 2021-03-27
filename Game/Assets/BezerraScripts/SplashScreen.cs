using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPressAny;

    SwitchMenu switchMenu;
    Color textColor;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;
        switchMenu = GetComponent<SwitchMenu>();
        textColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
            switchMenu.SwitchMenus();

        textColor.a = Mathf.PingPong(Time.time, 1);
        textPressAny.color = textColor;
    }
}
