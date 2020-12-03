using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
	private void Start()
	{
		PlayerController test = FindObjectOfType<PlayerController>();
		test.transform.position = this.transform.position;
	}
}