using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSound : MonoBehaviour
{
    public SoundEffect collectEffect;
    SFXPlayer sfxPlayer;

    private void Start()
    {
        sfxPlayer = gameObject.GetComponentInChildren<SFXPlayer>();
    }

    public void PlayCollect()
    {
        if (sfxPlayer != null)
        {
            sfxPlayer.PlaySound(collectEffect);
        }
    }
}
