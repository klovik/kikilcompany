using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string name = "ItemName";
    public int price = 0;
    public bool heavy = false;
    public PlayerInventory.Item itemType = PlayerInventory.Item.None;
}
