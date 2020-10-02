using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    public SoundEffect deathSound;
    public SoundEffect attackedSound;

    private SFXPlayer sfxPlayer;

    private void Start()
    {
        sfxPlayer = transform.GetComponentInChildren<SFXPlayer>();;
    }

    public void PlayDeath()
    {
        sfxPlayer.PlaySound(deathSound);
    }

    public void PlayAttacked()
    {
        sfxPlayer.PlaySound(attackedSound);
    }
}
