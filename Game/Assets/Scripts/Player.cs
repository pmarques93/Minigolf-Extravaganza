using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{

    //------------------------------------------------------------------------

    

    protected Ball ball;

    //------------------------------------------------------------------------

    private void Start()
    {

    }

    public void setBall(Ball ball)
    {
        this.ball = ball;
    }

    public enum Type
    {
        Human,
        CPU
    }
}
