using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
	public float jumpTakeOffSpeed = 7;

	public float maxSpeed = 7;
	private Animator animator;

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	protected override void ComputeVelocity()
	{
		Vector2 move = Vector2.zero;

		move.x = Input.GetAxis("Horizontal");

		if (Input.GetButtonDown("Jump") && grounded)
		{
			velocity.y = jumpTakeOffSpeed;
		}
		else if (Input.GetButtonUp("Jump"))
		{
			if (velocity.y > 0)
			{
				velocity.y *= 0.5f;
			}
		}

		bool flipSprite = spriteRenderer.flipX ? move.x > 0.0f : move.x < 0.0f;
		if (flipSprite)
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}

		animator.SetBool("grounded", grounded);
		animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
		animator.SetFloat("velocityY", velocity.y);

		targetVelocity = move * maxSpeed;
	}

	private void PlayFootstep() { }
}
