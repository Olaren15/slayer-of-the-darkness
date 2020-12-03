using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	public int life;
	public int maxLife;

	public Image[] hearts;
	public Sprite fullHeart;
	public Sprite emptyHeart;

	private PlayerController playerController;

	private void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
	}

	private void Update()
	{
		life = playerController.life;
		maxLife = playerController.maxLife;

		if (life > maxLife)
		{
			life = maxLife;
		}
		for (int i = 0; i < hearts.Length; i++)
		{
			if (i < life)
			{
				hearts[i].sprite = fullHeart;
			}
			else
			{
				hearts[i].sprite = emptyHeart;
			}

			if (i < maxLife)
			{
				hearts[i].enabled = true;
			}
			else
			{
				hearts[i].enabled = false;
			}
		}
	}

	public void AddHeart(int numberOfHeartsToAdd)
	{
		int newNoOfHearts = maxLife + numberOfHeartsToAdd;
		int maxNoOfHearts = hearts.Length;

		if (newNoOfHearts <= maxNoOfHearts)
		{
			maxLife = newNoOfHearts;
		}
	}
}