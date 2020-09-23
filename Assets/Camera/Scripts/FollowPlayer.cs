using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float followSpeed = 10.0f;
    public float minDistance = 0.25f;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        Vector3 cameraPosition = transform.position;

        Vector3 targetDirection = player.transform.position - cameraPosition;
        float velocity = targetDirection.magnitude <= minDistance ? 0.0f : targetDirection.magnitude * followSpeed;
        Vector3 targetPos = cameraPosition + targetDirection.normalized * (velocity * Time.deltaTime);

         transform.position = Vector3.Lerp(cameraPosition, targetPos, 0.25f);
    }
}
