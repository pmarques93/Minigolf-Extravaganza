using UnityEngine;

/// <summary>
/// Class responsible for calculating and drawing catapult trajetory.
/// </summary>
public class CatapultProjection : MonoBehaviour
{
    // Trajetory variables
    [SerializeField] private LineRenderer lineRender;
    [SerializeField] private Transform projectionStartPos;
    [SerializeField] private byte numberOfProjections;
    [SerializeField] private float spaceBetweenProjections;
    private Vector3 direction;

    // Components
    private Catapult catapult;

    private void Awake()
    {
        catapult = GetComponent<Catapult>();
    }

    private void Start()
    {
        lineRender.positionCount = numberOfProjections;
    }

    /// <summary>
    /// Applies positions to line render to calculate trajetory.
    /// </summary>
    private void FixedUpdate()
    {
        direction = (projectionStartPos.position + projectionStartPos.forward) - projectionStartPos.position;

        if (catapult.HasBall)
        {
            if (lineRender.enabled == false) lineRender.enabled = true;

            for (int i = 0; i < numberOfProjections; i++)
            {
                lineRender.SetPosition(i, ProjectionPosition(i * spaceBetweenProjections));
            }
        }
        else
        {
            if (lineRender.enabled) lineRender.enabled = false;
        }
    }

    /// <summary>
    /// Claculates trajetory.
    /// </summary>
    /// <param name="t">Distance of the projectiles.</param>
    /// <returns>Vector3 with position.</returns>
    private Vector3 ProjectionPosition(float t)
    {
        Vector3 position = (Vector3)
            projectionStartPos.position +
            (direction.normalized * catapult.EndBallForce * t) +
            0.1575f * Physics.gravity *
            (t * t);

        return position;
    }
}
