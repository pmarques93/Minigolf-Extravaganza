using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPFORWARD : MonoBehaviour
{
    [Header("MUST TURN ON BEFORE GAME START")]
    [SerializeField] private bool fastFoward;

    private static bool ff;
    public static bool FastForward => ff;
    private void Awake()
    {
        if (fastFoward) ff = true;
        else ff = false;
    }
    
}
