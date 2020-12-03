using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixCoin : MonoBehaviour
{
	private Text txtCoin;

	private void Start()
	{
		txtCoin = GameObject.Find("textCoin").GetComponent<Text>();
		string money = GameObject.Find("Player").GetComponent<Inventory>().money.ToString();
		txtCoin.text = money;
	}
}