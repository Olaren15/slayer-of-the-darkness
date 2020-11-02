using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableDoor : MonoBehaviour
{
	// Start is called before the first frame update
	private BoxCollider2D boxCollider;

	public int sceneID;

	private void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	private void Update()
	{
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (GameManager.controls.Player.Interact.ReadValue<float>() != 0)
			{
				SceneManager.LoadScene(sceneID);
			}
		}
	}
}