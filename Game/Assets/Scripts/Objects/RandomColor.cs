using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    [SerializeField]
    string materialName = "ColorVariationMoving";

    private void Awake() {
        Material[] mats = GetComponent<MeshRenderer>().materials;
        foreach(Material m in mats)
        {
            if (m.name.Contains(materialName))
            {
                m.SetFloat("_UVOffset", Random.Range(0f, 1f));
            }
                
        }
    }
}
