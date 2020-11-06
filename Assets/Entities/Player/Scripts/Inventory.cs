using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int money = 0;

    public void AddMoney(int moneyToAdd)
    {
        money += moneyToAdd;
    }
}
