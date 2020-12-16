using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
	public GameObject victorySound;
	private BossAttack _bossAttack;

	private void Start()
	{
		_bossAttack = GetComponent<BossAttack>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<PlayerController>().TakeDamage(1000);
		}
	}

	public void Die()
	{
		_bossAttack.enabled = false;
		StartCoroutine(nameof(PlayVictory));
	}

	private IEnumerator PlayVictory()
	{
		if (victorySound != null)
		{
			Instantiate(victorySound);
			yield return new WaitForSeconds(5.0f);
		}

		PauseMenu.GoBackToMainMenu();
	}
}