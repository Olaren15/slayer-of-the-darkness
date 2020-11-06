
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour, Collectable
{
	public const int value = 1;
	private CoinSound coinSound;
	public GameObject moneyCollectEffect;

	private void Start()
	{
		coinSound = gameObject.GetComponent<CoinSound>();
	}

	public void Collect(GameObject player)
	{
		player.GetComponent<Inventory>().AddMoney(value);
		Instantiate(moneyCollectEffect, new Vector3(transform.position.x, transform.position.y + transform.localScale.y),
				Quaternion.Euler(-45, 90, 0));
		coinSound.PlayCollect();
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		Destroy(gameObject, 3);
	}
}