using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Collectable
{
    public int coinAmount = 5;

    public void Collect(GameObject player)
    {
        player.GetComponent<Inventory>().AddMoney(coinAmount);
    }
}
