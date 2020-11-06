using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int money = 0;
    private Text txtCoin;

    private void Start()
    {
        txtCoin = GameObject.Find("textCoin").GetComponent<Text>();
    }

    public void AddMoney(int moneyToAdd)
    {
        money += moneyToAdd;
        txtCoin.text = money.ToString();
    }
}
