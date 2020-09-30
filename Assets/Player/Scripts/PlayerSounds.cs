using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
	public SoundEffect footstep1;
	public SoundEffect footstep2;

	private SFXPlayer sfxPlayer;
	private bool alternateSound = true;

	private void Start()
	{
		sfxPlayer = FindObjectOfType<SFXPlayer>();
	}

	public void PlayFootstep()
	{
		sfxPlayer.PlaySound(alternateSound ? footstep1 : footstep2);
		alternateSound = !alternateSound;
	}

	public void PlayJump()
	{
		sfxPlayer.PlaySound(footstep1);
	}

	public void PlayLanding()
	{
		sfxPlayer.PlaySound(footstep2);
	}
}
