using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScifiHole : MonoBehaviour
{
    [SerializeField] private Transform holeFinishPosition;

    private bool endLevel;
    private BallHandler ball;

    private void Start()
    {
        endLevel = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ball = other.GetComponent<BallHandler>();
            StartCoroutine(MoveBall(ball));
        }
    }

    private IEnumerator MoveBall(BallHandler ball)
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        ball.StopBall();
        ball.RB.isKinematic = true;

        while(Vector3.Distance(ball.transform.position, holeFinishPosition.position) > 0.1f)
        {
            ball.transform.position = 
                Vector3.MoveTowards(
                    ball.transform.position, 
                    holeFinishPosition.position, 
                    Time.fixedDeltaTime * 0.3f);

            yield return wffu;
        }
        endLevel = true;
        StartCoroutine(ball.FinishCourse());

        GetComponent<Animator>().SetTrigger("EndLevel");
    }

    private void FixedUpdate()
    {
        if (endLevel) ball.transform.position = holeFinishPosition.position;
    }
}
