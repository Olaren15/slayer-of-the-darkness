using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    public float MinPitch = 0.8f;
    public float maxPitch = 1.5f;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundEffect soundEffect)
    {
        audioSource.pitch = Random.Range(MinPitch, maxPitch);
        audioSource.clip = soundEffect.soundToPlay;
        audioSource.Play();
    }
}
