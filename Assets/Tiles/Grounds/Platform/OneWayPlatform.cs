using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class OneWayPlatform : MonoBehaviour
{
	public float waitTime = 0.5f;

	private TilemapCollider2D tilemapCollider;
	private PlayerController player;

	private void Start()
	{
		player = FindObjectOfType<PlayerController>();
		tilemapCollider = GetComponent<TilemapCollider2D>();

		GameManager.controls.Player.Crouch.performed += context => TryDisableCollider();
		GameManager.controls.Player.JumpPress.performed += context => TryDisableCollider();
	}

	private void TryDisableCollider()
	{
		if (player.IsGrounded)
		{
			StartCoroutine(DisableCollider());
		}
	}

	private IEnumerator DisableCollider()
	{
		tilemapCollider.enabled = false;
		yield return new WaitForSeconds(waitTime);
		tilemapCollider.enabled = true;
	}
}