using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] LevelSelect levelSelect;

    public void Activated()
    {
        levelSelect.GoToLevel(sceneName);
    }

}
