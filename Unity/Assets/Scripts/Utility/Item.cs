using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "ItemName";
    public bool priceless = false;
    public int price = 0;
    public bool heavy = false;
    public float weight = 1;
    public PlayerInventory.Item itemType = PlayerInventory.Item.None;
}
