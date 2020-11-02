using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int money = 0;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collectable collectable = collision.gameObject.GetComponent<Collectable>();
        if (collectable != null)
        {
            collectable.Collect(gameObject);
        }
    }

    public void AddMoney(int moneyToAdd)
    {
        money += moneyToAdd;
    }
}
