using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public int money = 0;
	private Text txtCoin;

	private void Start()
	{
	}

	public void AddMoney(int moneyToAdd)
	{
		txtCoin = GameObject.Find("textCoin").GetComponent<Text>();

		money += moneyToAdd;
		txtCoin.text = money.ToString();
	}
}