using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName = "ItemName";
    [FormerlySerializedAs("itemType")] public PlayerInventory.ItemId itemIdType = PlayerInventory.ItemId.None;
    [Header("Item Attributes")]
    [Tooltip ("Placement mode")]
    public bool holdable = true;
    [Tooltip ("Store item in inventory")]
    public bool storable = false;
    [Tooltip ("Item rotating in placement mode")]
    public bool rotatable = true;
    [Tooltip("Item in hand slot")]
    public bool handable = false;
    
    [Header("Item Usage")]
    [Tooltip ("Handed using")]
    public bool handUsable = false;
    [Tooltip ("Placed using")]
    public bool placeUsable = false;
    
    public delegate void UseDelegate();
    [Tooltip("Use delegate")]
    public UseDelegate useDelegate;

    private bool isBeingUse = false;
    
    [Header("Item Movement")]
    private bool isBeingHolded = false;
    private GameObject rayEnd;
    [HideInInspector] public bool canBePlacedNow = false;

    [Header("Item Rotation")]
    private float xRot, yRot, zRot;

    private bool inRotationMode = false;
    private GameObject rotationAxis;
    private GameObject rotationAxisGO = null;

    private void Start()
    {
        rotationAxis = Resources.Load<GameObject>("RotAxis");
        rayEnd = GameObject.FindGameObjectWithTag("rayEnd");
        if (handUsable || placeUsable)
        {
            useDelegate = delegate { UnknownDelegate(); };
            System.Reflection.MethodInfo method = this.GetType().GetMethod(itemIdType.ToString() + "Use");
            if (method != null)
            {
                useDelegate = (UseDelegate)Delegate.CreateDelegate(typeof(UseDelegate), this, method);
                print("ASSigned");
            }
            else
            {
                print($"Usage method {itemIdType.ToString()}Use not found!");
            }
        }
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
                rotationAxisGO.transform.rotation = new Quaternion(0, 0, 0, 0);
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
            if (rayEnd == null) rayEnd = GameObject.Find("rayEnd");
            transform.position = rayEnd.transform.position;
        }
        
        if(isBeingHolded) GetComponent<Outline>().enabled = canBePlacedNow;

        if (isBeingUse && itemIdType == PlayerInventory.ItemId.Boombox) BoomboxChecker();
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
        GetComponent<Outline>().enabled = false;
        GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        GetComponent<Outline>().OutlineColor = new Color(0, 0, 255, 1);
    }
    public void Store()
    {
        PlayerInventory.AddItem(itemIdType);
        PlayerInteraction.Outlines.Remove(gameObject.GetComponent<Outline>());
        Destroy(gameObject);
    }

    public void UnknownDelegate()
    {
        print("Delegate not set!");
    }
    public void BoomboxUse()
    {
        AudioSource audio = transform.GetComponentInChildren<AudioSource>();
        if (!isBeingUse)
        {
            isBeingUse = true;
            transform.GetComponentInChildren<Animator>().Play("Playing");
            AudioClip[] clips = Resources.LoadAll<AudioClip>("BoomboxAudio");
            AudioClip clip = Util.ArrayRandomChoice(clips);
            audio.PlayOneShot(clip);
        }
        else
        {
            transform.GetComponentInChildren<Animator>().Play("Idle");
            isBeingUse = false;
            audio.Stop();
        }

    }

    private void BoomboxChecker()
    {
        AudioSource audio = transform.GetComponentInChildren<AudioSource>();
        if (!audio.isPlaying)
        {
            transform.GetComponentInChildren<Animator>().Play("Idle");
            isBeingUse = false;
        }
    }
}
