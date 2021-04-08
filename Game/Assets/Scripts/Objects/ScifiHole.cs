using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScifiHole : MonoBehaviour
{
    [SerializeField] private Transform holeFinishPosition;

    private bool endLevel;
    private BallMovement ballMovement;

    private void Start()
    {
        endLevel = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ballMovement = other.GetComponent<BallMovement>();
            StartCoroutine(MoveBall(ballMovement));
        }
    }

    private IEnumerator MoveBall(BallMovement ballMovement)
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        ballMovement.StopBall();
        ballMovement.Rb.isKinematic = true;

        while(Vector3.Distance(ballMovement.transform.position, holeFinishPosition.position) > 0.1f)
        {
            ballMovement.transform.position = 
                Vector3.MoveTowards(
                    ballMovement.transform.position, 
                    holeFinishPosition.position, 
                    Time.fixedDeltaTime * 0.3f);

            yield return wffu;
        }
        endLevel = true;

        BallHandler ballHandler = ballMovement.GetComponent<BallHandler>();
        StartCoroutine(ballHandler.FinishCourse());

        GetComponent<Animator>().SetTrigger("EndLevel");
    }

    private void FixedUpdate()
    {
        if (endLevel) ballMovement.transform.position = holeFinishPosition.position;
    }
}
