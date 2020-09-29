using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
	public float jumpTakeOffSpeed = 10;
	public float maxSpeed = 7;
	public float ladderGrabDeadZone = 0.3f;
	public float climbSpeed = 5.0f;

	private Animator animator;
	private BoxCollider2D boxCollider;

	private ContactFilter2D ladderContactFilter;
	private readonly List<RaycastHit2D> ladderOverlaps = new List<RaycastHit2D>();

	private bool isFlipped;
	private bool attachedToLadder;
	private float lastLadderXPosition;

	private static readonly int AttachedToLadder = Animator.StringToHash("attachedToLadder");
	private static readonly int Grounded = Animator.StringToHash("grounded");
	private static readonly int VelocityX = Animator.StringToHash("velocityX");
	private static readonly int VelocityY = Animator.StringToHash("velocityY");

	private static readonly Vector2 ClimbingColliderOffset = new Vector2(0.0774f, 0.6871f);
	private static readonly Vector2 ClimbingColliderSize = new Vector2(0.5331f, 1.2515f);
	private static readonly Vector2 NormalColliderOffset = new Vector2(-0.0625f, 0.6871f);
	private static readonly Vector2 NormalColliderSize = new Vector2(0.8131f, 1.2515f);
	
	private void Start()
	{
		animator = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
		ladderContactFilter.useTriggers = true;
		ladderContactFilter.useLayerMask = false;
		ladderContactFilter.layerMask = Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("Ladders"));
		
		GameManager.controls.Player.JumpPress.performed += context => JumpPressed();
		GameManager.controls.Player.JumpRelease.performed += context => JumpReleased();
	}

	private void JumpPressed()
	{
		if (attachedToLadder)
		{
			DetachFromLadder();
		}

		if (grounded)
		{
			velocity.y = jumpTakeOffSpeed;
		}
	}

	private void JumpReleased()
	{
		if (velocity.y > 0.0f)
		{
			velocity.y *= 0.5f;
		}
	}

	private void DetachFromLadder()
	{
		attachedToLadder = false;
		disableGravity = false;

		boxCollider.offset = NormalColliderOffset;
		boxCollider.size = NormalColliderSize;
	}

	private void AttachToLadder()
	{
		rb2d.position = new Vector2 {x = lastLadderXPosition, y = rb2d.position.y};

		boxCollider.offset = ClimbingColliderOffset;
		boxCollider.size = ClimbingColliderSize;

		if (isFlipped)
		{
			Flip();
		}

		attachedToLadder = true;
		disableGravity = true;
	}

	private void Flip()
	{
		transform.rotation = Quaternion.Euler(0.0f, isFlipped ? 0.0f : 180.0f, 0.0f);
		isFlipped = !isFlipped;
	}

	protected override void ComputeVelocity()
	{
		UpdateLadderAttachment();
		targetVelocity = attachedToLadder ? LadderMovement() : GroundMovement();
		UpdateAnimatorVariables();
	}

	private Vector2 LadderMovement()
	{
		return new Vector2
		{
			x = 0.0f,
			y = GameManager.controls.Player.Climb.ReadValue<float>() * climbSpeed
		};
	}

	private Vector2 GroundMovement()
	{
		float inputX = GameManager.controls.Player.Move.ReadValue<float>();
		
		bool needToFlip = isFlipped ? inputX > 0.0f : inputX < 0.0f;
		if (needToFlip)
		{
			Flip();
		}

		return new Vector2(inputX * maxSpeed, 0.0f);
	}
	
	private void UpdateLadderAttachment()
	{
		if (IsOnLadder())
		{
			if (Mathf.Abs(GameManager.controls.Player.Climb.ReadValue<float>()) > ladderGrabDeadZone)
			{
				AttachToLadder();
			}
		}
		else
		{
			DetachFromLadder();
		}
	}

	private bool IsOnLadder()
	{
		rb2d.Cast(Vector2.zero, ladderContactFilter, ladderOverlaps, 0.0f);
		return ladderOverlaps.Any();
	}

	private void UpdateAnimatorVariables()
	{
		animator.SetBool(AttachedToLadder, attachedToLadder);
		animator.SetBool(Grounded, grounded);
		animator.SetFloat(VelocityX, Mathf.Abs(velocity.x) / maxSpeed);
		animator.SetFloat(VelocityY, velocity.y);
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Ladder"))
		{
			lastLadderXPosition = other.bounds.center.x - 0.2f;
		}
	}
}
