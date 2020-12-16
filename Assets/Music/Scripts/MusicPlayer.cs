using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	public static MusicPlayer instance;

	private AudioSource audioSource;
	public AudioClip[] musics;
	private int musicIndex = 0;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
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
		musicIndex = 1;
		audioSource.clip = musics[musicIndex];
		audioSource.Play();
	}

	public void MainMenuMusicPlayer()
	{
		audioSource.Stop();
		musicIndex = 0;
		audioSource.clip = musics[musicIndex];
		audioSource.Play();
	}
}