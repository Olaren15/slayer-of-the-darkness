using UnityEngine;

public class BossAttack : MonoBehaviour
{
	public GameObject attackPrefab;
	public float attackDelay = 3.0f;

	private PlayerController playerController;

	private float timeBeforeAttack = 0.0f;

	private void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
	}

	private void Update()
	{
		if (!playerController.isDead)
		{
			if ((timeBeforeAttack -= Time.deltaTime) <= 0.0f)
			{
				timeBeforeAttack = attackDelay;
				AttackPlayer();
			}
		}
	}

	private void AttackPlayer()
	{
		Vector3 attackPosition = playerController.gameObject.transform.position;

		RaycastHit2D hit2D = Physics2D.Raycast(new Vector2(attackPosition.x, attackPosition.y + 0.1f), Vector2.down);
		if (hit2D)
		{
			attackPosition.y -= hit2D.distance;
		}

		Instantiate(attackPrefab, attackPosition, Quaternion.identity);
	}
}
