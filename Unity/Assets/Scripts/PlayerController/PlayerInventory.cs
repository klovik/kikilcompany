using System;
using System.Collections;
using System.Collections.Generic;
using Fragsurf.Movement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static Item[] inventory = new Item[16];
    public GameObject inventoryPanel;
    private bool inventoryMenuOpened = false;
    public GameObject[] inventorySlots;
    
    public enum Item {
        None, HPPrinter
    }

    private void Start()
    {
        GameObject inventorySlotsParent = inventoryPanel.transform.GetChild(0).gameObject;
        for (int i = 0; i < inventorySlotsParent.transform.childCount; i++)
        {
            inventorySlots[i] = inventorySlotsParent.transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyBindings.openInventory))
        {
            ChangeInventoryState(!inventoryMenuOpened);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if(AddItem(Item.HPPrinter) == 1) print("Failed to add an item");
        }
        
        if (inventoryMenuOpened)
        {
            UpdateInventorySlotsIcons();
        }
    }

    private void UpdateInventorySlotsIcons()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            Image currentSlotImg = inventorySlots[i].transform.GetChild(0).GetComponent<Image>();
            if (inventory[i] == Item.None)
            {
                RemoveImageInSlot(currentSlotImg);
            }
            else
            {
                UpdateImageInSlot(currentSlotImg, inventory[i].ToString());
            }
        }
    }



    public static int AddItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == Item.None)
            {
                inventory[i] = item;
                return 0;
            }
        }
        return 1;
    }

    private void UpdateImageInSlot(Image slot, string spriteName)
    {
        Sprite newSprite = Resources.Load<Sprite>($"Sprites/Items/{spriteName}");
        slot.sprite = newSprite;
        slot.color = new Color(1, 1, 1, 1);
    }
    private void RemoveImageInSlot(Image slot)
    {
        slot.color = new Color(1, 1, 1, 0);
        slot.sprite = null;
    }
    private void ChangeInventoryState(bool state)
    {
        if (state)
        {
            inventoryMenuOpened = true;
            Cursor.lockState = CursorLockMode.None;
            inventoryPanel.GetComponent<Animator>().Play("Open");
            SurfCharacter.movementEnabled = false;
        }
        else
        {
            inventoryMenuOpened = false;
            Cursor.lockState = CursorLockMode.Locked;
            inventoryPanel.GetComponent<Animator>().Play("Close");
            SurfCharacter.movementEnabled = true;
        }
    }
    public void Open() => ChangeInventoryState(true);
    public void Close() => ChangeInventoryState(false);


}
