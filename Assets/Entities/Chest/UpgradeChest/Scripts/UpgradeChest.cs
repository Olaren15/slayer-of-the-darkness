using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeChest : Chest
{
	public enum Upgrades
	{
		Heart,
		HealthRestoration,
		AttackDamage
	}

	public Upgrades upgrade;

	public int heartUpgradePrice = 3;
	public int healthRestorationPrice = 4;
	public int attackDamageUpgradePrice = 5;

	public int noOfHeartsToAdd = 1;
	public int attackDamageUpgrade = 1;

	public GameObject healthAndHeartUpgradeCollectEffect;
	public GameObject attackDamagehUpgradeCollectEffect;

	public AudioClip upgradeIsBoughtAudioClip;
	public AudioClip upgradeCannotBeBuyAudioClip;

	public new void Start()
	{
		IniStart();
		SetIconSprite();
	}

	public override void Collect(GameObject player)
	{
		if (!isCollected)
		{
			int upgradePrice = GetUpgradePrice(upgrade);
			bool upgradeIsDone = false;

			if (VerifyIfPlayerHasEnoughMoneyToUpgrade(upgradePrice, player))
			{
				if (VerifyIfPlayerCanBeUpgrade(upgrade, player))
				{
					UpgradePlayer(upgrade, player);

					isCollected = true;
					animator.SetTrigger("Open");

					GameObject effectToShow = GetEffectToShow(upgrade);
					Instantiate(effectToShow, new Vector3(transform.position.x, transform.position.y + transform.localScale.y), Quaternion.Euler(-45, 90, 0));
					player.GetComponent<Inventory>().AddMoney(upgradePrice * -1);

					upgradeIsDone = true;
				}
			}

			if (upgradeIsDone)
			{
				audioSource.clip = upgradeIsBoughtAudioClip;
			}
			else
			{
				audioSource.clip = upgradeCannotBeBuyAudioClip;
			}

			audioSource.Play();
		}
	}

	private void SetIconSprite()
	{
		SpriteRenderer iconSpriteRenderer = gameObject.GetComponentsInChildren<SpriteRenderer>().Where(go => go.gameObject != this.gameObject).FirstOrDefault();
		Sprite sprite = Resources.Load<Sprite>(GetIconPath());
		iconSpriteRenderer.sprite = sprite;
	}

	private string GetIconPath()
	{
		string iconPath = "Sprites/";

		switch (upgrade)
		{
			case Upgrades.Heart:
				iconPath += "HealthUpgradeIcon";
				break;

			case Upgrades.HealthRestoration:
				iconPath += "HealthRestorationIcon";
				break;

			case Upgrades.AttackDamage:
				iconPath += "AttackDamageUpgradeIcon";
				break;

			default:
				iconPath += "AttackDamageUpgradeIcon";
				break;
		}

		return iconPath;
	}

	private int GetUpgradePrice(Upgrades selectedUpgrade)
	{
		int upgradePrice = 0;

		switch (selectedUpgrade)
		{
			case Upgrades.Heart:
				upgradePrice = heartUpgradePrice;
				break;

			case Upgrades.HealthRestoration:
				upgradePrice = healthRestorationPrice;
				break;

			case Upgrades.AttackDamage:
				upgradePrice = attackDamageUpgradePrice;
				break;

			default:
				break;
		}

		return upgradePrice;
	}

	private GameObject GetEffectToShow(Upgrades selectedUpgrade)
	{
		GameObject effectToShow = collectEffect;

		switch (selectedUpgrade)
		{
			case Upgrades.Heart:
				effectToShow = healthAndHeartUpgradeCollectEffect;
				break;

			case Upgrades.HealthRestoration:
				effectToShow = healthAndHeartUpgradeCollectEffect;
				break;

			case Upgrades.AttackDamage:
				effectToShow = attackDamagehUpgradeCollectEffect;
				break;

			default:
				break;
		}

		return effectToShow;
	}

	private bool VerifyIfPlayerHasEnoughMoneyToUpgrade(int upgradePrice, GameObject player)
	{
		return player.GetComponent<Inventory>().money >= upgradePrice;
	}

	private void UpgradePlayer(Upgrades selectedUpgrade, GameObject player)
	{
		switch (upgrade)
		{
			case Upgrades.Heart:
				player.GetComponent<PlayerController>().AddHeart(noOfHeartsToAdd);
				break;

			case Upgrades.HealthRestoration:
				player.GetComponent<PlayerController>().RestoreHealth();
				break;

			case Upgrades.AttackDamage:
				player.GetComponent<PlayerController>().UpgradeAttackDamage(attackDamageUpgrade);
				break;

			default:
				break;
		}
	}

	private bool VerifyIfPlayerCanBeUpgrade(Upgrades selectedUpgrade, GameObject player)
	{
		bool playerCanBeUpgrade = false;

		switch (upgrade)
		{
			case Upgrades.Heart:
				playerCanBeUpgrade = VerifyIfPlayerCanAddHearts();
				break;

			case Upgrades.HealthRestoration:
				playerCanBeUpgrade = VerifyIfPlayerCanRestoreItsHealth(player);
				break;

			case Upgrades.AttackDamage:
				playerCanBeUpgrade = VerifyIfPlayerCanUpgradeAttackDamage(player);
				break;

			default:
				break;
		}

		return playerCanBeUpgrade;
	}

	private bool VerifyIfPlayerCanAddHearts()
	{
		// Verify if player doesn't already have the max number of hearts
		Health health = FindObjectOfType<Health>();

		return (health.maxLife + noOfHeartsToAdd) <= health.hearts.Length;
	}

	private bool VerifyIfPlayerCanRestoreItsHealth(GameObject player)
	{
		// Verify if player doesn't already have it's full health
		return FindObjectOfType<Health>().maxLife > player.GetComponent<PlayerController>().life;
	}

	private bool VerifyIfPlayerCanUpgradeAttackDamage(GameObject player)
	{
		// Verify if player doesn't alreay have the max attack damage.
		PlayerController playerController = player.GetComponent<PlayerController>();

		return playerController.maxAttackDamage >= (attackDamageUpgrade + playerController.attackDamage);
	}
}