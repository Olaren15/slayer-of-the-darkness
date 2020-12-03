using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSlider : MonoBehaviour
{
	private AudioSource audioSource;
	private float musicVolume = 0.25f;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		audioSource.volume = musicVolume;
	}

	public void SetVolume(float sliderVolume)
	{
		musicVolume = sliderVolume;
	}
}