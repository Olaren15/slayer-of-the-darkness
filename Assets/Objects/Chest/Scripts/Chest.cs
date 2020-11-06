using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Collectable
{
    private int coinAmount;
    private Animator animator;
    private AudioSource audioSource;
    public bool isCollected = false;
    public GameObject collectEffect;

    public void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        coinAmount = System.Convert.ToInt32(Random.Range(0, 10));
    }

    public void Collect(GameObject player)
    {
        if (!isCollected)
        {
            isCollected = true;
            audioSource.Play();
            animator.SetTrigger("Open");
            Instantiate(collectEffect, new Vector3(transform.position.x, transform.position.y + transform.localScale.y), Quaternion.Euler(-45, 90, 0));
            player.GetComponent<Inventory>().AddMoney(coinAmount);
        }
    }
}
