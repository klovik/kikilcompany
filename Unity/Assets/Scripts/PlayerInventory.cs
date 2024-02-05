using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{

    public static Item[] inventory = new Item[3];
    public GameObject[] inventorySlots = new GameObject[3];
    public static int activeSlot = 0;

    private Vector3 activeInventorySlotScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 notActiveInventorySlotScale = new Vector3(0.4f, 0.4f, 0.4f);

    public Text inventoryText;

    void Start()
    {
        UpdateInventorySlotsSizes();
    }

    private void Update()
    {
        UpdateInventoryText();
        RenderItemsInHotbar();

        //scrolling inventory slots
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (activeSlot == 2)
                activeSlot = 0;
            else
                activeSlot++;
            UpdateInventorySlotsSizes();
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (activeSlot == 0)
                activeSlot = 2;
            else
                activeSlot--;
            UpdateInventorySlotsSizes();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            activeSlot = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            activeSlot = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            activeSlot = 2;
    }

    private void UpdateInventorySlotsSizes()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if (i == activeSlot)
                inventorySlots[i].transform.localScale = activeInventorySlotScale;
            else
                inventorySlots[i].transform.localScale = notActiveInventorySlotScale;     
        }
    }

    public static int AddItem(Item itemType)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == Item.None)
            {
                inventory[i] = itemType; print($"Added {itemType.ToString()} to slot #{i}"); return 0;
            }
        }
        return 1;
    }

    public static int SetItem(Item itemType, int index)
    {
        inventory[index] = itemType;
        return 0;
    }
    public static int RemoveItemAtIndex(int index)
    {
        inventory[index] = Item.None;
        return 0;
    }
    public static int RemoveItemByName(Item itemType)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == itemType)
            {
                inventory[i] = Item.None; return 0;
            }  
        }
        return 1;
    }

    private void UpdateInventoryText()
    {
        inventoryText.text = $"{inventory[0]}, {inventory[1]}, {inventory[2]}";
    }

    private void RenderItemsInHotbar()
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            Image imageSlot = inventorySlots[i].transform.GetChild(0).gameObject.GetComponent<Image>();
            if (inventory[i] != Item.None)
            {
                string itemName = inventory[i].ToString();
                Sprite itemSprite = Resources.Load<Sprite>($"Sprites/Items/{itemName}");
                imageSlot.sprite = itemSprite;
                imageSlot.color = new Color(1, 1, 1, 1);
            }
            else
            {
                imageSlot.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public enum Item
    {
        None, Pioneer, ECord, ViperRAM
    }
}
