using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    public float minPitch = 0.8f;
    public float maxPitch = 1.5f;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundEffect soundEffect)
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.clip = soundEffect.soundToPlay;
        audioSource.Play();
    }
}
