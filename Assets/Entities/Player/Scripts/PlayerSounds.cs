using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
	public SoundEffect footstep1;
	public SoundEffect footstep2;
	public SoundEffect jumping;
	public SoundEffect damaged;
	public SoundEffect dying;

	private SFXPlayer sfxPlayer;
	private bool alternateSound = true;

	private void Start()
	{
		sfxPlayer = transform.GetComponentInChildren<SFXPlayer>();
	}

	public void PlayFootstep()
	{
		sfxPlayer.PlaySound(alternateSound ? footstep1 : footstep2);
		alternateSound = !alternateSound;
	}

	public void PlayLanding()
	{
		sfxPlayer.PlaySound(footstep2);
	}

	public void PlayDamaged()
	{
		sfxPlayer.PlaySound(damaged);
	}

	public void PlayDie()
	{
		sfxPlayer.PlaySound(dying);
	}

	public void PlayJump()
	{
		sfxPlayer.PlaySoundAtPitch(jumping, 1);
	}

	public void PlayDoubleJump()
	{
		sfxPlayer.PlaySoundAtPitch(jumping, 1.25f);
	}
}
