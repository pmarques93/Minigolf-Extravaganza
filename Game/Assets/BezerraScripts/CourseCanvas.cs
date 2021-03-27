using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CourseCanvas : MonoBehaviour
{
    [SerializeField]
    private string courseName;

    [SerializeField]
    private TextMeshProUGUI textPlayerName;

    [SerializeField]
    private TextMeshProUGUI textPuttsCounter;

    [SerializeField]
    private TextMeshProUGUI textCourse;

    [SerializeField]
    private Image imgFF;


    private float fastTimer;

    private Color fastColor;

    private void Start()
    {
        fastTimer = 0;
        fastColor = Color.white;
        SetCourseName();
    }

    private void Update()
    {
        UpdateFastForward();
    }

    public void SetCourseName()
    {
        //textCourse.text = courseName;
    }

    private void UpdateFastForward()
    {

        if (Time.timeScale > 1)
        {
            fastTimer += Time.deltaTime;
            fastColor.a = Mathf.PingPong(fastTimer, 1);
        }
        else
        {
            fastTimer = 0;
            fastColor.a = 0;
        }

        imgFF.color = fastColor;
    }

    public void SetPlayerName(string pName, Color color)
    {
        textPlayerName.text = pName;
        textPlayerName.color = color;
    }

    public void SetPuttsCounter(int n)
    {
        textPuttsCounter.text = n.ToString();
    }


}
