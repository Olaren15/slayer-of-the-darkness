using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowTarget : MonoBehaviour
{
	[Range(1.0f, 25.0f)]
	public float followSpeed = 10.0f;

	[Range(0.001f, 1.0f)]
	public float stiffnessFactor = 0.5f;

	public float minDistance = 0.05f;
	public Vector3 offset = new Vector3(0.0f, 1.0f, 12.5f);
	public GameObject target;

	private Camera cameraComponent;
	private WorldLimits worldLimits;

	private void Start()
	{
		// follow player if nothing is specified
		if (!target)
		{
			target = GameObject.FindGameObjectWithTag("Player");
		}

		cameraComponent = GetComponent<Camera>();
		worldLimits = FindObjectOfType<WorldLimits>();
	}

	private void FixedUpdate()
	{
		Vector3 cameraPosition = transform.position;

		Vector3 targetDirection = target.transform.position - cameraPosition + offset;
		float velocity = targetDirection.magnitude <= minDistance ? 0.0f : targetDirection.magnitude * followSpeed;
		Vector3 targetPos = cameraPosition + targetDirection.normalized * (velocity * Time.deltaTime);

		transform.position = Vector3.Lerp(cameraPosition, targetPos, stiffnessFactor) - CalculateWorldLimitsOffset();
	}

	private Vector3 CalculateWorldLimitsOffset()
	{
		Vector3 bottomLeft = cameraComponent.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, -offset.z));
		Vector3 topRight =
			cameraComponent.ScreenToWorldPoint(
				new Vector3(cameraComponent.pixelWidth, cameraComponent.pixelHeight, -offset.z));

		Bounds bounds = new Bounds();
		bounds.SetMinMax(bottomLeft, topRight);

		return worldLimits.GetPositionCorrectionOffset(bounds);
	}
}
