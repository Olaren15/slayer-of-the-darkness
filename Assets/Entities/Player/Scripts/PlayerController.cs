using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : PhysicsObject, IDamageable
{
	public static PlayerController instance;
	public bool IsGrounded => grounded;

	public float jumpTakeOffSpeed = 10;
	public float maxSpeed = 7;
	public float ladderGrabDeadZone = 0.3f;
	public float climbSpeed = 5.0f;

	public Transform attackPoint;
	public float attackRange;
	public LayerMask enemyLayer;
	public LayerMask interactableLayer;

	private Animator animator;
	private ContactFilter2D ladderContactFilter;
	private readonly List<RaycastHit2D> ladderOverlaps = new List<RaycastHit2D>();
	private SpriteRenderer spriteRenderer;
	private PlayerSounds playerSounds;

	public int life = 3;
	public int maxLife = 3;
	public int attackDamage = 1;
	public int maxAttackDamage = 10;
	public float maxImmunityTime = 2.0f;

	[NonSerialized]
	public float remainingImmunityTime;

	[NonSerialized]
	public bool isDead;

	private bool isFlipped;
	private bool canDoubleJump;
	private bool attachedToLadder;
	private bool isOnLadder;
	private float lastLadderXPosition;

	private static readonly int AttachedToLadder = Animator.StringToHash("attachedToLadder");
	private static readonly int Grounded = Animator.StringToHash("grounded");
	private static readonly int VelocityX = Animator.StringToHash("velocityX");
	private static readonly int VelocityY = Animator.StringToHash("velocityY");
	private static readonly int DieTrigger = Animator.StringToHash("die");
	private static readonly int AttackTrigger = Animator.StringToHash("attack");
	private static readonly int DamageTrigger = Animator.StringToHash("takeDamage");

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		playerSounds = GetComponent<PlayerSounds>();
		ladderContactFilter.useTriggers = true;
		ladderContactFilter.useLayerMask = true;
		ladderContactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("Ladders")));

		if (instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	private void JumpPressed()
	{
		// jumping from ladder
		if (attachedToLadder)
		{
			DetachFromLadder();
			velocity.y = jumpTakeOffSpeed;
			playerSounds.PlayJump();
		}
		// normal jump
		else if (grounded)
		{
			velocity.y = jumpTakeOffSpeed;
			playerSounds.PlayJump();
		}
		// double jump
		else if (canDoubleJump && !attachedToLadder)
		{
			velocity.y = jumpTakeOffSpeed;
			playerSounds.PlayDoubleJump();
			canDoubleJump = false;
		}
	}

	private void JumpReleased()
	{
		if (velocity.y > 0.0f)
		{
			velocity.y *= 0.5f;
		}
	}

	private void CrouchPressed()
	{
		if (attachedToLadder)
		{
			DetachFromLadder();
		}
	}

	private void AttackPressed()
	{
		if (instance)
		{
			animator.SetTrigger(AttackTrigger);

			Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

			foreach (Collider2D enemy in hitEnemies)
			{
				enemy.GetComponent<IDamageable>()?.TakeDamage(attackDamage);
			}
		}
	}

	private void DetachFromLadder()
	{
		attachedToLadder = false;
		disableGravity = false;
	}

	private void AttachToLadder()
	{
		// reset the double jump state
		canDoubleJump = true;

		rb2d.position = new Vector2(lastLadderXPosition, rb2d.position.y);

		if (isFlipped)
		{
			Flip();
		}

		attachedToLadder = true;
		disableGravity = true;
	}

	private void Flip()
	{
		//spriteRenderer.flipX = !spriteRenderer.flipX;
		transform.rotation = Quaternion.Euler(0.0f, isFlipped ? 0.0f : 180.0f, 0.0f);
		isFlipped = !isFlipped;
	}

	protected override void PhysicsObjectUpdate()
	{
		if (remainingImmunityTime > 0)
		{
			remainingImmunityTime -= Time.deltaTime;

			if (remainingImmunityTime <= 0)
			{
				SetOpacity(1.0f);
			}
		}

		UpdateLadderAttachment();

		if (IsGrounded)
		{
			canDoubleJump = true;
		}

		targetVelocity = attachedToLadder ? LadderMovement() : GroundMovement();
		UpdateAnimatorVariables();
	}

	private Vector2 LadderMovement()
	{
		return new Vector2(0.0f, GameManager.controls.Player.Climb.ReadValue<float>() * climbSpeed);
	}

	private Vector2 GroundMovement()
	{
		float inputX = GameManager.controls.Player.Move.ReadValue<float>();

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("DefaultAttack") && rb2d.velocity.y == 0)
		{
			inputX = 0;
		}

		bool needToFlip = isFlipped ? inputX > 0.0f : inputX < 0.0f;
		if (needToFlip)
		{
			Flip();
		}

		return new Vector2(inputX * maxSpeed, 0.0f);
	}

	private void Die()
	{
		isDead = true;
		GameManager.controls.Player.Disable();
		animator.SetTrigger(DieTrigger);
		FindObjectOfType<DeathMenu>()?.Show();
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

	private void UpdateAnimatorVariables()
	{
		animator.SetBool(AttachedToLadder, attachedToLadder);
		animator.SetBool(Grounded, grounded);
		animator.SetFloat(VelocityX, Mathf.Abs(velocity.x) / maxSpeed);
		animator.SetFloat(VelocityY, velocity.y);
	}

	private bool IsOnLadder()
	{
		rb2d.Cast(Vector2.zero, ladderContactFilter, ladderOverlaps, 0.0f);
		return ladderOverlaps.Any();
	}

	private void SetOpacity(float alpha)
	{
		Color tmp = spriteRenderer.color;
		tmp.a = alpha;
		spriteRenderer.color = tmp;
	}

	public void TakeDamage(int damage)
	{
		if (!isDead && remainingImmunityTime <= 0.0f)
		{
			life -= damage;

			if (life <= 0)
			{
				Die();
			}
			else
			{
				animator.SetTrigger(DamageTrigger);
				SetOpacity(0.5f);
				remainingImmunityTime = maxImmunityTime;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Ladder"))
		{
			lastLadderXPosition = other.transform.position.x;
		}

		if (other.CompareTag("Coin"))
		{
			Collectable collectable = other.gameObject.GetComponent<Collectable>();
			collectable?.Collect(gameObject);
		}

		if (other.CompareTag("Lever"))
		{
			other.gameObject.GetComponent<Lever>().ActivateNext();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (GameManager.controls.Player.Interact.ReadValue<float>() != 0)
		{
			collision.gameObject.GetComponent<Collectable>()?.Collect(gameObject);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}

	public void RegisterAction()
	{
		GameManager.controls.Player.JumpPress.performed += context => JumpPressed();
		GameManager.controls.Player.JumpRelease.performed += context => JumpReleased();
		GameManager.controls.Player.Crouch.performed += context => CrouchPressed();
		GameManager.controls.Player.Attack.performed += context => AttackPressed();
	}

	public void RestoreHealth()
	{
		int maxNoOfHearts = FindObjectOfType<Health>().maxLife;

		life = maxNoOfHearts;
	}

	public void AddHeart(int numberOfHeartsToAdd)
	{
		int newNoOfHearts = maxLife + numberOfHeartsToAdd;
		int maxNoOfHearts = FindObjectOfType<Health>().hearts.Length;

		if (newNoOfHearts <= maxNoOfHearts)
		{
			maxLife = newNoOfHearts;
		}
	}

	public void UpgradeAttackDamage(int attackDamageUpdate)
	{
		int newAttackDamage = attackDamage + attackDamageUpdate;

		if (newAttackDamage <= maxAttackDamage)
		{
			attackDamage = newAttackDamage;
		}
	}
}