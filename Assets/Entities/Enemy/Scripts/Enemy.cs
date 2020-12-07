using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
	public GameObject deathEffect;
	public Animator animator;
	public Transform attackPoint;
	public LayerMask playerLayer;

	public int life = 3;
	public int attackDamage = 1;
	public float attackRange;
	private bool isDead;
	private bool isAttacking;
	public float attackCooldown = 1.0f;

	private static readonly int AttackTrigger = Animator.StringToHash("Attack");
	private static readonly int TakeDamageTrigger = Animator.StringToHash("TakeDamage");
	private static readonly int DieTrigger = Animator.StringToHash("Die");

	public enum AnimationEvent
	{
		AttackDone
	}

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (!isDead)
		{
			if (!isAttacking)
			{
				CheckAttack();
			}
		}
	}

	private void CheckAttack()
	{
		Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

		foreach (Collider2D player in hitPlayers)
		{
			IDamageable damageablePlayer = player.GetComponent<IDamageable>();
			if (damageablePlayer != null && player.GetComponent<PlayerController>().remainingImmunityTime <= 0 &&
					!player.GetComponent<PlayerController>().isDead)
			{
				Attack(damageablePlayer);
			}
		}
	}

	private void Attack(IDamageable entity)
	{
		isAttacking = true;
		animator.SetTrigger(AttackTrigger);
		entity.TakeDamage(attackDamage);
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
			animator.SetTrigger(TakeDamageTrigger);
		}
	}

	private void Die()
	{
		isDead = true;
		Instantiate(deathEffect, new Vector3(transform.position.x, transform.position.y + transform.localScale.y),
			Quaternion.identity);

		animator.SetTrigger(DieTrigger);
	}

	public void DestroyEnemy()
    {
		Destroy(gameObject);
    }

	private void AnimationObserver(AnimationEvent e)
	{
		switch (e)
		{
			case AnimationEvent.AttackDone:
				isAttacking = false;
				break;
			default:
				break;
		}
	}
}
