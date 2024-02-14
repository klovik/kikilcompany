using System;
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
    public bool rotatable = true;
    [FormerlySerializedAs("itemType")] public PlayerInventory.ItemId itemIdType = PlayerInventory.ItemId.None;
    
    [Header("Item Movement")]
    private bool isBeingHolded = false;
    private GameObject rayEnd;
    public bool canBePlacedNow = false;

    [Header("Item Rotation")]
    private float xRot, yRot, zRot;

    private bool inRotationMode = false;
    public GameObject rotationAxis;
    public GameObject rotationAxisGO = null;

    private void Start()
    {
        rotationAxis = Resources.Load<GameObject>("RotAxis");
        rayEnd = GameObject.Find("rayEnd");
    }

    private void ChangeRotationModeState(bool state)
    {
        inRotationMode = state;
        if (state)
        {
            print($"[{gameObject.name}] Entering rotomode");
            if (rotationAxisGO == null)
            {
                rotationAxisGO = Instantiate(rotationAxis, transform.position, Quaternion.identity);
                rotationAxisGO.transform.SetParent(transform);
            }
        }
        else
        {
            print($"[{gameObject.name}] Exiting rotomode");
            if(rotationAxisGO != null) Destroy(rotationAxisGO);
            rotationAxisGO = null;
        }
    }

    public void StartRotationMode() => ChangeRotationModeState(true);
    public void ExitRotationMode() => ChangeRotationModeState(false);
    private void Update()
    {
        if (isBeingHolded)
        {
            transform.position = rayEnd.transform.position;
        }
        
        if(isBeingHolded) GetComponent<Outline>().enabled = canBePlacedNow;
    }

    private void OnCollisionEnter(Collision other)
    {
        canBePlacedNow = true;
    }

    private void OnCollisionExit(Collision other)
    {
        canBePlacedNow = false;
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
        isBeingHolded = true;
    }

    public void StopHolding()
    {
        print("Stop Holding");
        isBeingHolded = false;
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
