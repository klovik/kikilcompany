using System;
using System.Collections;
using System.Collections.Generic;
using Fragsurf.Movement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static ItemId[] inventory = new ItemId[16];
    public static ItemId handSlot = ItemId.None;
    public GameObject inventoryPanel;
    private bool inventoryMenuOpened = false;
    public GameObject[] inventorySlots;
    public static int contextedSlotIndex = -1;
    public static GameObject player;

    private static GameObject contextMenu = null;
    public GameObject contextMenuDropPrefab;
    public GameObject contextMenuDropHoldPrefab;
    public GameObject contextMenuDropHoldHandPrefab;
    public Text cursorText;
    public Canvas parentCanvas;
    public static int hoveredSlotIndex = -1;

    public static int lastEmptySlot = 0;

    [Header("Hand slot")]
    public GameObject itemHolder;
    private GameObject handedItem;

    public Text contextedText, hoveredText;
    

    private void UpdateCursorTextPosition()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        movePos.y += 16;
        cursorText.transform.position = parentCanvas.transform.TransformPoint(movePos);
    }
    public string GetItemName(ItemId item)
    {
        Item itemGO = Resources.Load<Item>($"Trash/{item}");
        return itemGO.itemName;
    }
    public void OnInventorySlotHover(int slotIndex)
    {
        hoveredSlotIndex = slotIndex;
        if (inventory[slotIndex] != ItemId.None && slotIndex != contextedSlotIndex)
        {
            cursorText.text = GetItemName(inventory[slotIndex]);
        }
    }
    public void OnInventorySlotUnHover()
    {
        hoveredSlotIndex = -1;
        cursorText.text = "";
    }
    private void NumerateSlots()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            string curName = inventorySlots[i].name;
            inventorySlots[i].name = $"[#{i}] {curName}";
        }
    }
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
        AssignInventorySlots();
        NumerateSlots();
    }
    private void Update()
    {
        lastEmptySlot = CalculateLastEmptySlot();
        RenderItemInHand();
        UpdateCursorTextPosition();
        if (inventoryMenuOpened) UpdateInventorySlotsIcons();
        if (hoveredSlotIndex == contextedSlotIndex) cursorText.text = "";
        if (Input.GetKeyDown(KeyBindings.openInventory)) ChangeInventoryState(!inventoryMenuOpened);
        if (Input.GetKeyDown(KeyCode.J)) AddItem(ItemId.HPPrinter);
        contextedText.text = $"contexted: {contextedSlotIndex}";
        hoveredText.text = $"hovered: {hoveredSlotIndex}";
    }

    private int CalculateLastEmptySlot()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == ItemId.None) return i;
        }

        return -1;
    }
    private void AssignInventorySlots()
    {
        GameObject inventorySlotsParent = inventoryPanel.transform.GetChild(0).gameObject;
        for (int i = 0; i < inventorySlotsParent.transform.childCount; i++)
        {
            inventorySlots[i] = inventorySlotsParent.transform.GetChild(i).gameObject;
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
    public static void CloseContextMenu()
    {
        if (contextMenu != null) Destroy(contextMenu);
        contextedSlotIndex = -1;
        contextMenu = null;
    }
    public void CreateInventoryContextMenu(GameObject slot)
    {
        int curIndex = GetSlotIndexByGameObject(slot);
        if (contextedSlotIndex != -1) CloseContextMenu();
        
        if (inventory[curIndex] == ItemId.None || curIndex == contextedSlotIndex)
        {
            CloseContextMenu();
            return;
        }
        
        contextedSlotIndex = GetSlotIndexByGameObject(slot);
        Item contextedItem = Resources.Load<GameObject>($"Trash/{inventory[contextedSlotIndex]}").GetComponent<Item>();
        GameObject contextMenuPrefab = null;
        
        if (contextedItem.holdable && contextedItem.handable) contextMenuPrefab = contextMenuDropHoldHandPrefab;
        else if (contextedItem.holdable && !contextedItem.handable) contextMenuPrefab = contextMenuDropHoldPrefab;
        else contextMenuPrefab = contextMenuDropPrefab;

        contextMenu = Instantiate(contextMenuPrefab, slot.transform);

        for (int i = 0; i < contextMenu.transform.childCount; i++)
        {
            switch (contextMenu.transform.GetChild(i).name)
            {
                case "Drop":
                    contextMenu.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    contextMenu.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(ContextMenuDropItem);
                    break;
                case "Hold":
                    contextMenu.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    contextMenu.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(ContextMenuHoldItem);
                    break;
                case "Hand":
                    contextMenu.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    contextMenu.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(ContextMenuHandItem);
                    break;
            }
        }
        
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
        if (contextedSlotIndex == -1)
        {
            print("Got -1 index in CTXDrop");
            return;
        }
        DropItemBySlotIndex(contextedSlotIndex);
        CloseContextMenu();
    }
    public static void ContextMenuHoldItem()
    {
        if (contextedSlotIndex == -1)
        {
            print("Got -1 index in CTXHold");
            return;
        }
        GameObject dropped = DropItemBySlotIndex(contextedSlotIndex);
        PlayerInteraction.PIStartHolding(dropped);
        CloseContextMenu();
    }
    public static void ContextMenuHandItem()
    {
        if (contextedSlotIndex == -1)
        {
            print("Got -1 index in CTXHand");
            return;
        }
        if (handSlot == ItemId.None)
        {
            handSlot = inventory[contextedSlotIndex];
            inventory[contextedSlotIndex] = ItemId.None;
        }
        CloseContextMenu();
    }
    private void RenderItemInHand()
    {
        if (handSlot == ItemId.None && handedItem != null) //if there is no held item, but handeditem exists
        {
            Destroy(handedItem);
            handedItem = null;
        }
        else if(handSlot != ItemId.None & handedItem == null) //if there is held item, but no handeditem exists
        {
            GameObject item = Resources.Load<GameObject>($"ItemHolder/{handSlot}");
            handedItem = Instantiate(item, itemHolder.transform);
            //item.transform.SetParent(itemHolder.transform);
        }
        else if (handedItem != null && handSlot.ToString() != handedItem.name) //if held item != handeditem
        {
            Destroy(handedItem);
            GameObject item = Resources.Load<GameObject>($"ItemHolder/{handSlot}");
            handedItem = Instantiate(item, itemHolder.transform);
        }
    }
    public static GameObject DropItemBySlotIndex(int slotIndex)
    {
        print($"Got {slotIndex} slotIndex");
        print($"That's {inventory[slotIndex]} item.");
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
            cursorText.text = "";
            CloseContextMenu();
        }
    }
    public void Open() => ChangeInventoryState(true);
    public void Close() => ChangeInventoryState(false);


}
