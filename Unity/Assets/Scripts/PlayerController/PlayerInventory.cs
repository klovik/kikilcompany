using System;
using System.Collections;
using System.Collections.Generic;
using Fragsurf.Movement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static ItemId[] inventory = new ItemId[16];
    public GameObject inventoryPanel;
    private bool inventoryMenuOpened = false;
    public GameObject[] inventorySlots;
    public static int contextedSlotIndex = -1;
    public static GameObject player;

    private static GameObject contextMenu = null;

    public GameObject contextMenuPrefab;
    
    public enum ItemId {
        None,
        ECord,
        ElectroEbator,
        HPPrinter,
        Pioneer,
        Poco,
        TV,
        ViperRam
    }

    private void Start()
    {
        player = GameObject.Find("Player");
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
            if(AddItem(ItemId.HPPrinter) == 1) print("Failed to add an item");
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
            if (inventory[i] == ItemId.None)
            {
                RemoveImageInSlot(currentSlotImg);
            }
            else
            {
                UpdateImageInSlot(currentSlotImg, inventory[i].ToString());
            }
        }
    }

    private static void CloseContextMenu()
    {
        Destroy(contextMenu);
        contextedSlotIndex = -1;
        contextMenu = null;
    }
    public void CreateInventoryContextMenu(GameObject slot)
    {
        int curIndex = GetSlotIndexByGameObject(slot);
        
        if (inventory[curIndex] == ItemId.None || curIndex == contextedSlotIndex)
        {
            print("I won't create context menu for empty or current slot, you stupid nigger!");
            CloseContextMenu();
            return;
        }
        
        contextedSlotIndex = GetSlotIndexByGameObject(slot);
        contextMenu = Instantiate(contextMenuPrefab);
        contextMenu.transform.parent = slot.transform;
        contextMenu.transform.localPosition = new Vector3(0, 0, 0);
        contextMenu.transform.localScale = new Vector3(1, 1, 1);

    }   
    public int GetSlotIndexByGameObject(GameObject go)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == go) return i;
        }

        print("Can't found an inventory slot");
        return -1;
    }
    public static int AddItem(ItemId itemId)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == ItemId.None)
            {
                inventory[i] = itemId;
                return 0;
            }
        }
        return 1;
    }

    public static void ContextMenuDropItem()
    {
        DropItem(contextedSlotIndex);
        CloseContextMenu();
    }

    public static void ContextMenuHoldItem()
    {
        GameObject dropped = DropItem(contextedSlotIndex);
        PlayerInteraction.PIStartHolding(dropped);
        CloseContextMenu();
    }
    
    public static GameObject DropItem(int slotIndex)
    {
        string itemName = inventory[slotIndex].ToString();
        GameObject itemPrefab = Resources.Load<GameObject>($"Trash/{itemName}");
        inventory[slotIndex] = ItemId.None;
        GameObject instantiatedItem = Instantiate(itemPrefab);
        instantiatedItem.transform.position = player.transform.position;
        return instantiatedItem;
    }
    private void UpdateImageInSlot(Image slot, string spriteName)
    {
        Sprite newSprite = Resources.Load<Sprite>($"Sprites/Items/{spriteName}");
        slot.sprite = newSprite;
        slot.color = new Color(1, 1, 1, 1);
    }
    private void UpdateImageInSlot(GameObject slot, string spriteName)
    {
        Image slotImg = slot.transform.GetChild(0).GetComponent<Image>();
        Sprite newSprite = Resources.Load<Sprite>($"Sprites/Items/{spriteName}");
        slotImg.sprite = newSprite;
        slotImg.color = new Color(1, 1, 1, 1);
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
