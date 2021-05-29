using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObjectRotation : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.eulerAngles += new Vector3(3, 3, 3) * Time.fixedDeltaTime;
    }
}
