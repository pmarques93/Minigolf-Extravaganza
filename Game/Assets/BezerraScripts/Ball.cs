using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float lineLength = 1;

    [SerializeField]
    protected float maxPower = 20;

    [SerializeField]
    private GameObject prefabOobParticles;

    [SerializeField]
    private GameObject prefabSpawnParticles;

    [SerializeField]
    private GameObject prefabConfettiParticles;

    private CourseManager courseManager;

    //------------------------------------------------------------------------

    private readonly float MIN_VELOCITY = 0.01f;
    
    //------------------------------------------------------------------------

    private Rigidbody ball;

    private LineRenderer line;

    private float angle;

    private Vector3 previousPosition;

    private bool isGrounded;

    public int Putts { get; private set; }

    public bool WaitingPutt { get; set; }

    public bool FinishedCourse { get; set; }

    private float stoppedTime;

    private bool isActiveBall;

    private float timeScale;

    private bool finished;

    //------------------------------------------------------------------------

    // Start is called before the first frame update
    void Awake()
    {
        finished = false;
        ball = GetComponent<Rigidbody>();
        ball.maxAngularVelocity = 1000;
        line = GetComponentInChildren<LineRenderer>();
        courseManager = GameObject.Find("CourseManager").GetComponent<CourseManager>();
    }

    public void SetupBall(Transform startPosition, Color color)
    {
        PlaceBall(startPosition.position);
        previousPosition = startPosition.position;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        angle = startPosition.rotation.eulerAngles.y;

        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
        line.material.SetColor("_BaseColor", color);

        Putts = 0;
        WaitingPutt = true;
        FinishedCourse = false;

        gameObject.SetActive(false);
    }

    private void PlaceBall(Vector3 position)
    {
        transform.position = position;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;

        SpawnParticles(prefabSpawnParticles, 3);

        GetComponent<MeshRenderer>().enabled = true;

        ball.isKinematic = false;
    }

    public void SetLineActive(bool active)
    {
        line.enabled = active;
    }

    // Update is called once per frame
    void Update()
    {
        if (FinishedTurn() && isActiveBall)
        {
            isActiveBall = false;
            courseManager.EndTurn();
        }
    }

    public void GoToPos(Transform pos)
    {
        transform.position = pos.position;
    }

    public void UpdateAngle(float dir)
    {
        angle += dir;
        UpdateLineAngle();
    }

    public bool IsMoving()
    {
        return ball.velocity.magnitude >= MIN_VELOCITY || !isGrounded;
    }


    private bool HasStopped()
    {
        if (IsMoving())
            stoppedTime = 0;
        else
        {
            stoppedTime += Time.deltaTime;
        }
        if (stoppedTime >= courseManager.maxStoppedTime)
            return true;
        return false;
    }

    public void StartTurn()
    {
        if (finished)
            return;

        isActiveBall = true;
        WaitingPutt = true;
        stoppedTime = 0;
        line.enabled = true;
        timeScale = Time.timeScale;
        Time.timeScale = 1.0f;
    }

    private bool FinishedTurn()
    {
        if (WaitingPutt)
            return false;

        if (!HasStopped())
            return false;

        return true;
    }

    private void UpdateLineAngle()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength);
    }

    public void Shoot(float power)
    {
        if(!IsMoving())
        {
            VisualEffect vfx = SpawnParticles(prefabSpawnParticles, 3);
            vfx.SetFloat("Strength", power);

            line.enabled = false;
            WaitingPutt = false;
            Putts++;

            //if(GameManager.currentMode == Gamemode.Mode.Training)
            //{
                previousPosition = transform.position;
                previousPosition += Vector3.up * 2;
            //}
            ball.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * power * maxPower, ForceMode.Impulse);
            Time.timeScale = timeScale;
        }
    }

    private VisualEffect SpawnParticles(GameObject particles, float duration)
    {
        GameObject particle = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(particle, duration);
        return particle.GetComponent<VisualEffect>();
    }

    private IEnumerator FinishCourse()
    {
        finished = true;
        SpawnParticles(prefabConfettiParticles, 4);
        GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(4.5f);

        FinishedCourse = true;
        courseManager.SaveScore(Putts);
        gameObject.SetActive(false);
    }

    private IEnumerator ResetBall()
    {
        GetComponent<MeshRenderer>().enabled = false;

        SpawnParticles(prefabOobParticles, 3);

        Putts++;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;

        yield return new WaitForSeconds(1f);

        PlaceBall(previousPosition);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hole"))
            StartCoroutine(FinishCourse());     
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("OoB"))
        {
            StartCoroutine(ResetBall());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }


}
