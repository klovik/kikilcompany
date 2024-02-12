using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float rayLength = 3f;
    public Text inputHintText;
    public Transform rayEnd;
    public static List<Outline> Outlines = new List<Outline>();
    public static bool isHoldingItem = false;
    private static GameObject holdingItem = null;

    private bool doNotFuckingChangeHoldingStateThisFrame = false;

    private void Update()
    {
        doNotFuckingChangeHoldingStateThisFrame = false;
        ClearOutlines();
        ClearInputHint();
        
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        rayEnd.transform.position = ray.GetPoint(rayLength);

        int playerLayer = LayerMask.GetMask("Player");
        int layerMask = ~playerLayer;
        
        if (isHoldingItem)
        {
            AddToInputHint("[E] Confirm");
            AddToInputHint("[F] Store");
            if (Input.GetKeyDown(KeyCode.E))
            {
                PIStopHolding();
                doNotFuckingChangeHoldingStateThisFrame = true;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                PIStoreHoldingItem();
                doNotFuckingChangeHoldingStateThisFrame = true;
            }
        }

        if (Physics.Raycast(ray, out hit, rayLength, layerMask))
        {
            rayEnd.transform.position = hit.point;
            if (!isHoldingItem)
            {
                switch (hit.collider.tag)
                {
                    case "Item":
                        Item item = hit.collider.GetComponent<Item>();
                        Outline outline = hit.collider.GetComponent<Outline>();
                        if (item.holdable) AddToInputHint("[E] Pickup");
                        if (item.storable) AddToInputHint("[F] Store");
                        outline.enabled = true;
                        Outlines.Add(outline);
                        if (Input.GetKeyDown(KeyCode.E) && !doNotFuckingChangeHoldingStateThisFrame && hit.collider.GetComponent<Item>().holdable)
                        {
                            PIStartHolding(hit.collider.gameObject);
                        }
                        else if (Input.GetKeyDown(KeyCode.F) && !doNotFuckingChangeHoldingStateThisFrame && hit.collider.GetComponent<Item>().storable)
                        {
                            item.Store();
                        }
                        break;
                }
            }
            
            Debug.DrawRay(cam.transform.position, cam.transform.forward * rayLength, Color.red);
        }
        else
        {
            ClearOutlines();
            Debug.DrawRay(cam.transform.position, cam.transform.forward * rayLength, Color.green);
        }
    }

    public static void PIStartHolding(GameObject item)
    {
        holdingItem = item.gameObject;
        isHoldingItem = true;
        item.GetComponent<Item>().StartHolding();
    }

    public static void PIStopHolding()
    {
        holdingItem.GetComponent<Item>().StopHolding();
        holdingItem = null;
        isHoldingItem = false;
    }

    public static void PIStoreHoldingItem()
    {
        holdingItem.GetComponent<Item>().Store();
        isHoldingItem = false;
    }
    
    private void ClearInputHint()
    {
        inputHintText.text = "";
    }

    private void ClearOutlines()
    {
        foreach(Outline ol in Outlines)
        {
            ol.enabled = false;
        }
        Outlines.Clear();
    }
    
    private void AddToInputHint(string text)
    {
        inputHintText.text += $"{text}\n";
    }
}
