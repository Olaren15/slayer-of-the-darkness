using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
	public float jumpTakeOffSpeed = 10;
	public float maxSpeed = 7;
	public float ladderGrabDeadZone = 0.3f;
	public float climbSpeed = 5.0f;

	private Animator animator;
	private Controls controls;
	private BoxCollider2D boxCollider;
	
	private bool isFlipped;
	private bool isOnLadder;
	private bool attachedToLadder;
	private float lastLadderXPosition;

	private static readonly int AttachedToLadder = Animator.StringToHash("attachedToLadder");
	private static readonly int VerticalMovement = Animator.StringToHash("verticalMovement");
	private static readonly int Grounded = Animator.StringToHash("grounded");
	private static readonly int VelocityX = Animator.StringToHash("velocityX");
	private static readonly int VelocityY = Animator.StringToHash("velocityY");

	private static readonly Vector2 ClimbingColliderOffset = new Vector2(0.0774f, 0.6871f);
	private static readonly Vector2 ClimbingColliderSize = new Vector2(0.5331f, 1.2515f);
	private static readonly Vector2 NormalColliderOffset = new Vector2(-0.0625f, 0.6871f);
	private static readonly Vector2 NormalColliderSize = new Vector2(0.8131f, 1.2515f);

	private void Awake()
	{
		animator = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
		
		controls = new Controls();
		controls.Enable();
		controls.Player.JumpPress.performed += context => JumpPressed();
		controls.Player.JumpRelease.performed += context => JumpReleased();
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

	private Vector2 Move(float inputX)
	{
		bool needToFlip = isFlipped ? inputX > 0.0f : inputX < 0.0f;
		if (needToFlip)
		{
			Flip();
		}
		
		return new Vector2(inputX * maxSpeed, 0.0f);
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
		rb2d.position = new Vector2
		{
			x = lastLadderXPosition,
			y = rb2d.position.y
		};

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
		float verticalMovement = controls.Player.Climb.ReadValue<float>();

		if (attachedToLadder)
		{
			rb2d.position += new Vector2
			{
				x = 0.0f, 
				y = verticalMovement * climbSpeed * Time.deltaTime
			};
		}
		else
		{
			if (isOnLadder)
			{
				if (Mathf.Abs(verticalMovement) > ladderGrabDeadZone)
				{
					AttachToLadder();
				}
			}
			
			targetVelocity = Move(controls.Player.Move.ReadValue<float>());
		}

		animator.SetBool(AttachedToLadder, attachedToLadder);
		animator.SetFloat(VerticalMovement, Mathf.Abs(verticalMovement));
		animator.SetBool(Grounded, grounded);
		animator.SetFloat(VelocityX, Mathf.Abs(velocity.x) / maxSpeed);
		animator.SetFloat(VelocityY, velocity.y);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Ladder"))
		{
			isOnLadder = true;
			lastLadderXPosition = other.bounds.center.x - 0.2f;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Ladder"))
		{
			isOnLadder = false;
		}
	}
}
