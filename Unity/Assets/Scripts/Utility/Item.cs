using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName = "ItemName";
    public bool holdable = true;
    public bool storable = false;
    public bool usable = false;
    [FormerlySerializedAs("itemType")] public PlayerInventory.ItemId itemIdType = PlayerInventory.ItemId.None;
    
    [Header("Item Movement")]
    private bool isHolding = false;
    private GameObject rayEnd;

    private void Start()
    {
        rayEnd = GameObject.Find("rayEnd");
    }

    private void Update()
    {
        if (isHolding)
        {
            transform.position = rayEnd.transform.position;
        }
    }
        
    public void StartHolding()
    {
        //PlayerInteraction.Outlines.Remove(gameObject.GetComponent<Outline>());
        print("Start Holding");
        gameObject.tag = "Untagged";
        GetComponent<Outline>().enabled = true;
        GetComponent<Outline>().OutlineColor = new Color(0, 255, 0, 1);
        GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
        //DisableAllChildrenRender()
        gameObject.layer = LayerMask.NameToLayer("Player");
        isHolding = true;
    }

    public void StopHolding()
    {
        print("Stop Holding");
        isHolding = false;
        gameObject.layer = 0;
        //DisableAllChildrenRender()
        gameObject.tag = "Item";
        GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        GetComponent<Outline>().OutlineColor = new Color(0, 0, 255, 1);
        GetComponent<Outline>().enabled = false;
    }

    private void DisableAllChildrenRender()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().enabled = false;
        }
    }
    private void EnableAllChildrenRender()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().enabled = true;
        }
    }

    public void Store()
    {
        PlayerInventory.AddItem(itemIdType);
        PlayerInteraction.Outlines.Remove(gameObject.GetComponent<Outline>());
        Destroy(gameObject);
    }
}
