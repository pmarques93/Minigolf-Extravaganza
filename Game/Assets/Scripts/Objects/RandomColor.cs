using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    [SerializeField]
    string materialName = "ColorVariationMoving";

    [SerializeField]
    private bool useGlobalColor = false;

    public static float? globalColor = null;

    private void Awake() {
        if (globalColor == null)
        {
            globalColor = Random.Range(0f, 1f);
        }

        Material[] mats = GetComponent<MeshRenderer>().materials;
        foreach(Material m in mats)
        {
            if (m.name.Contains(materialName))
            {
                if (!useGlobalColor)
                    m.SetFloat("_UVOffset", Random.Range(0f, 1f));

                else
                    m.SetFloat("_UVOffset", (float)globalColor);
            }
        }
    }
}
