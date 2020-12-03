using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	private AudioSource audioSource;
	public AudioClip[] musics;
	private int musicIndex = 0;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		int numberOfMusicPlayer = FindObjectsOfType<MusicPlayer>().Length;
		if (numberOfMusicPlayer > 1)
		{
			Destroy(gameObject); //on le supprime
		}
		else
		{
			DontDestroyOnLoad(gameObject);
		}
	}

	public void ChangeMusic()
	{
		audioSource.Stop();
		if (musicIndex < musics.Length - 1)
		{
			musicIndex++;
		}
		audioSource.clip = musics[musicIndex];
		audioSource.Play();
	}

	public void RestartMusicPlayer()
	{
		audioSource.Stop();
		musicIndex = 0;
		audioSource.clip = musics[musicIndex];
		audioSource.Play();
	}
}