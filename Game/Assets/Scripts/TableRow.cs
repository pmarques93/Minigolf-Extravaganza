using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableRow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text rowName;

    [SerializeField]
    private HorizontalLayoutGroup scores;

    [SerializeField]
    private TMP_Text tableCell;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetRow(string pName, string[] values, Color color)
    {
        SetName(pName, color);
        foreach (var str in values)
        {
            CreateCol(str, color);
        }
    }

    private void SetName(string pName, Color color)
    {
        rowName.text = pName;
        rowName.color = color;
    }

    private void CreateCol(string value, Color color)
    {
        TMP_Text field = Instantiate<TMP_Text>(tableCell);
        field.rectTransform.SetParent(scores.transform);
        field.text = value;
        field.color = color;
        field.rectTransform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
