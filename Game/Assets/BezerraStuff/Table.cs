using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField]
    private TableRow tableRowPrefab;

    private void Start()
    {

    }

    public void CreateRow(string name, string[] values)
    {
        CreateRow(name, values, Color.white);
    }

    public void CreateRow(string name, string[] values, Color color)
    {
        TableRow row = Instantiate<TableRow>(tableRowPrefab);
        row.SetRow(name, values, color);
        row.transform.SetParent(transform);
        row.transform.localScale = Vector3.one;
    }
}
