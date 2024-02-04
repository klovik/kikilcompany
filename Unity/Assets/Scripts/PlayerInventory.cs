using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public Item[] inventory = new Item[3];
    public GameObject[] inventorySlots = new GameObject[3];
    public int activeSlot = 0;

    private Vector3 activeInventorySlotScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 notActiveInventorySlotScale = new Vector3(0.4f, 0.4f, 0.4f);

    void Start()
    {
        UpdateInventorySlotsSizes();
    }

    private void Update()
    {
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
    public enum Item
    {
        None, Pioneer
    }
}
