using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableDoor : MonoBehaviour
{
	private AudioSource sound;
	private MusicPlayer music;
	public Animator transitionAnim;

	private void Start()
	{
		sound = GetComponent<AudioSource>();
		music = FindObjectOfType<MusicPlayer>();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (GameManager.controls.Player.Interact.ReadValue<float>() != 0)
			{
				sound.Play();
				StartCoroutine(LoadScene());
			}
		}
	}

	private IEnumerator LoadScene()
	{
		transitionAnim.SetTrigger("transitionEnd");
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		if (music != null)
		{
			music.ChangeMusic();
		}
	}
}