using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
	public SoundEffect footstep1;
	public SoundEffect footstep2;
	private bool alternateSound = true;

	public SoundEffect damaged;
	public SoundEffect dying;

	private SFXPlayer sfxPlayer;

	private void Start()
	{
		sfxPlayer = gameObject.transform.Find("SFXPlayer").gameObject.GetComponent<SFXPlayer>();
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
		sfxPlayer.PlaySound(footstep1);
	}
}
