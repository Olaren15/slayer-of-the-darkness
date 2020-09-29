using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlatformerController : PhysicsObject, IDamageable
{
	public float jumpTakeOffSpeed = 10;
	public float maxSpeed = 7;

	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private Controls controls;
	private new Collider2D collider;
	private bool isFlipped;
	
	public Transform attackPoint;
	public float attackRange;
	public LayerMask enemyLayer;

	public int life = 3;
	public int attackDamage = 1;

	private static readonly int GroundedParameter = Animator.StringToHash("grounded");
	private static readonly int VelocityXParameter = Animator.StringToHash("velocityX");
	private static readonly int VelocityYParameter = Animator.StringToHash("velocityY");

	private enum AnimationState
    {
		AttackEnded
    }

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		collider = GetComponent<Collider2D>();
		
		controls = new Controls();
		controls.Enable();
		controls.Player.JumpPress.performed += context => JumpPressed();
		controls.Player.JumpRelease.performed += context => JumpReleased();
		controls.Player.Attack.performed += context => Attack();
	}

    private void JumpPressed() 
	{
		if (grounded)
		{
			velocity.y = jumpTakeOffSpeed;
			//animator.SetBool("attacking", false);
		}
	}

	private void JumpReleased()
	{
		if (velocity.y > 0.0f)
		{
			velocity.y *= 0.5f;
		}
	}

	private void Attack()
    {
		animator.SetTrigger("attack");

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
			var damageableEnemy = enemy.GetComponent<IDamageable>();
			if (damageableEnemy != null)
            {
				damageableEnemy.TakeDamage(attackDamage);
			}
				
        }
	}

	private Vector2 Move(float inputX)
	{
		bool needToFlip = isFlipped ? inputX > 0.0f : inputX < 0.0f;

		if (needToFlip)
		{
			Flip();
		}

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DefaultAttack") && rb2d.velocity.y == 0)
        {
			inputX = 0;
        }

        return new Vector2(inputX * maxSpeed, 0.0f);
	}

	private void Flip()
    {
		// spriteRenderer.flipX = !spriteRenderer.flipX;
		transform.rotation = Quaternion.Euler(0.0f, isFlipped ? 0.0f : 180.0f, 0.0f);
		isFlipped = !isFlipped;
	}

	protected override void ComputeVelocity()
	{
		animator.SetBool(GroundedParameter, grounded);
		animator.SetFloat(VelocityXParameter, Mathf.Abs(velocity.x) / maxSpeed);
		animator.SetFloat(VelocityYParameter, velocity.y);

		targetVelocity = Move(controls.Player.Move.ReadValue<float>());
	}

    private void OnDrawGizmosSelected()
    {
		if (attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
		life -= damage;

		if (life <= 0)
        {
			Die();
		}
		else
		{
			animator.SetTrigger("takeDamage");
		}
    }

	private void Die()
    {
		animator.SetTrigger("die");


		spriteRenderer.enabled = false;
	}
}
