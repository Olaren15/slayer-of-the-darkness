using Unity.Mathematics;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float MaxVelocity = 1.0f;
	public float Speed = 10.0f;

	private float HorizontalVelocity;

	private Rigidbody2D RigidBody;

	private void Start()
	{
		RigidBody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		HorizontalVelocity = Input.GetAxis("Horizontal") * Speed;
	}

	private void FixedUpdate()
	{
		float targetHorizontalVelocity = math.clamp(RigidBody.velocity.x + HorizontalVelocity, -MaxVelocity, MaxVelocity);

		RigidBody.velocity = new Vector2(targetHorizontalVelocity, RigidBody.velocity.y);
	}
}
