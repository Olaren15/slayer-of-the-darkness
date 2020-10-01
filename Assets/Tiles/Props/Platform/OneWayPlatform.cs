using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class OneWayPlatform : MonoBehaviour
{
	public float waitTime = 0.5f;

	private void Start()
	{
		GameManager.controls.Player.GoDown.performed += context => GoDown();
	}

	private void GoDown()
	{
		StartCoroutine(FallTimer());
	}

	private IEnumerator FallTimer()
	{
		GetComponent<TilemapCollider2D>().enabled = false;
		yield return new WaitForSeconds(waitTime);
		GetComponent<TilemapCollider2D>().enabled = true;
	}
}