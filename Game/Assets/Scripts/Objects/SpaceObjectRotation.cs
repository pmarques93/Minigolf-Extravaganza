using UnityEngine;

public class SpaceObjectRotation : MonoBehaviour
{
    private Vector3 rotationVec;

    private void Start()
    {
        rotationVec = new Vector3(3, 3, 3);
    }

    private void FixedUpdate()
    {
        transform.Rotate(rotationVec * Time.fixedDeltaTime);
    }
}
