using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
	public float gravityModifier = 1f;
	public float minGroundNormalY = .65f;

	protected const float MinMoveDistance = 0.001f;
	protected const float ShellRadius = 0.01f;

	protected ContactFilter2D contactFilter;
	protected Vector2 groundNormal;
	protected Rigidbody2D rb2d;
	protected readonly List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>();

	protected Vector2 targetVelocity;
	protected Vector2 velocity;
	protected bool grounded;
	protected bool disableGravity = false;

	private void OnEnable()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
	}

	private void Update()
	{
		targetVelocity = Vector2.zero;

		// Freeze y position when gravity is disabled
		if (disableGravity)
		{
			rb2d.constraints |= RigidbodyConstraints2D.FreezePositionY;
		}
		else
		{
			rb2d.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
		}

		ComputeVelocity();
	}

	protected virtual void ComputeVelocity() { }

	private void FixedUpdate()
	{
		if (disableGravity)
		{
			velocity.y = targetVelocity.y;
		}
		else
		{
			velocity += Physics2D.gravity * (gravityModifier * Time.deltaTime);
		}

		velocity.x = targetVelocity.x;
		grounded = false;

		Vector2 deltaPosition = velocity * Time.deltaTime;
		Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
		Vector2 move = moveAlongGround * deltaPosition.x;

		Movement(move, false);
		move = Vector2.up * deltaPosition.y;
		Movement(move, true);
	}

	private void Movement(Vector2 move, bool yMovement)
	{
		float distance = move.magnitude;
		if (distance > MinMoveDistance)
		{
			rb2d.Cast(move, contactFilter, hitBufferList, distance + ShellRadius);
			foreach (RaycastHit2D hit in hitBufferList)
			{
				Vector2 currentNormal = hit.normal;
				if (currentNormal.y > minGroundNormalY)
				{
					grounded = true;
					if (yMovement)
					{
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				float projection = Vector2.Dot(velocity, currentNormal);
				if (projection < 0)
				{
					velocity -= projection * currentNormal;
				}

				float modifiedDistance = hit.distance - ShellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}

		rb2d.position += move.normalized * distance;
	}
}
