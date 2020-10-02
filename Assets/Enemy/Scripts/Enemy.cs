using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public GameObject deathEffect;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask playerLayer;

    public int life = 3;
    public int attackDamage = 1;
    private bool isDead = false;
    private bool isAttacking = false;
    public float attackCooldown = 1.0f;

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
            var damageablePlayer = player.GetComponent<IDamageable>();
            if (damageablePlayer != null && player.GetComponent<PlayerPlatformerController>().remainingImmunityTime <= 0 && !player.GetComponent<PlayerPlatformerController>().isDead)
            {
                Attack(damageablePlayer);
            }
        }
    }

    private void Attack(IDamageable entity)
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
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
            animator.SetTrigger("TakeDamage");
        }
    }

    private void Die() 
    {
        isDead = true;
        Instantiate(deathEffect, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + gameObject.transform.localScale.y), Quaternion.identity);
        animator.SetTrigger("Die");
        
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
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
