using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

public class HumanPlayer : Player
{
    [SerializeField]
    private Image powerBar;

    [SerializeField]
    private float changeAngleSpeed = 60;

    private float powerTimer;

    private float power;

    private bool isCharging;

    private bool shootHold = true;

    public InputMaster Controls { get; private set; }

    //------------------------------------------------------------------------

    // Start is called before the first frame update
    void Awake()
    {
        Controls = new InputMaster();

        // Hold and release
        if (shootHold)
        {
            Controls.Player.ShootStart.performed += _ => SetCharging(true);
            Controls.Player.ShootEnd.performed += _ => Shoot();
        }

        else
        {
            //Click and click
            Controls.Player.ShootStart.performed += _ => ShootCommand();
        }

        isCharging = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotateAngle(Controls.Player.Direction.ReadValue<float>());

        if (isCharging)
            PowerUp();

        //else
        //    ResetPower();
    }

    private void ShootCommand()
    {
        if (!isCharging)
        {
            SetCharging(true);
        }
        else
        {
            Shoot();
        }
    }

    private void SetCharging(bool value)
    {
            isCharging = value;
    }

    private void RotateAngle(float dir)
    {
        if (ball != null)
            ball.UpdateAngle(changeAngleSpeed * Time.deltaTime * dir);
    }

    private void PowerUp()
    {
        if (ball.IsMoving())
            return;

        powerTimer += Time.deltaTime;
        power = Mathf.PingPong(powerTimer, 1);
        powerBar.fillAmount = power;
        power = Mathf.Clamp(power, 0.1f, 1);
    }

    private void Shoot()
    {
        if (Time.timeScale != 0f)
        {
            ball.Shoot(power);
            ResetPower();
        }
    }

    private void ResetPower()
    {
        SetCharging(false);
        powerTimer = 0;
        powerBar.fillAmount = 0;
        power = 0;
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
