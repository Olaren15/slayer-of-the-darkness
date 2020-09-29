using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Range(1.0f, 25.0f)]
    public float followSpeed = 10.0f;
    public float minDistance = 0.25f;
    [Range(0.001f, 1.0f)]
    public float stiffnessFactor = 0.25f;
    public Vector3 offset = new Vector3(0.0f, 0.2f, 0.0f);
    public GameObject target;

    private void Start()
    {
        // follow player if nothing is specified
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void FixedUpdate()
    {
        Vector3 cameraPosition = transform.position;

        Vector3 targetDirection = target.transform.position - cameraPosition;
        float velocity = targetDirection.magnitude <= minDistance ? 0.0f : targetDirection.magnitude * followSpeed;
        Vector3 targetPos = cameraPosition + offset + targetDirection.normalized * (velocity * Time.deltaTime);

        transform.position = Vector3.Lerp(cameraPosition, targetPos, stiffnessFactor);
    }
}
