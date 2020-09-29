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

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        //animator.SetTrigger("attack");

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            var damageablePlayer = player.GetComponent<IDamageable>();
            if (damageablePlayer != null)
            {
                damageablePlayer.TakeDamage(attackDamage);
            }

        }
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
        Instantiate(deathEffect, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + gameObject.transform.localScale.y), Quaternion.identity);
        animator.SetTrigger("Die");
        
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }
}
