using UnityEngine;

public class Sword : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<IDamageable>()?.TakeDamage(5);
		}
	}
}
