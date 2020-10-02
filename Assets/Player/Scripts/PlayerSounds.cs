using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public SoundEffect footstep1;
    public SoundEffect footstep2;
    public SoundEffect damaged;
    public SoundEffect dying;

    private SFXPlayer sfxPlayer;
    private bool playFoot1 = true;
    private void Start()
    {
        sfxPlayer = gameObject.transform.Find("SFXPlayer").gameObject.GetComponent<SFXPlayer>();
    }

    public void PlayFootstep()
    {
        sfxPlayer.PlaySound(playFoot1 ? footstep1 : footstep2);
        playFoot1 = !playFoot1;
    }

    public void PlayJump()
    {
        sfxPlayer.PlaySound(footstep1);
    }

    public void PlayLanding() {
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
}
