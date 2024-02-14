using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float rayLength = 3f;
    public float rotationStrength = 4f;
    public Text inputHintText;
    public Transform rayEnd;
    public static List<Outline> Outlines = new List<Outline>();
    public static bool isHoldingItem = false;
    private static GameObject holdingItem = null;
    private bool inRotationMode = false;
    private rotationAxis currentRotationAxis = rotationAxis.None;

    private bool doNotFuckingChangeHoldingStateThisFrame = false;
    private bool doNotFuckingChangeRotatingStateThisFrame = false;
    public Canvas parentCanvas;

    private enum rotationAxis
    {
        None, X, Y, Z
    }

    private void Update()
    {
        doNotFuckingChangeHoldingStateThisFrame = false;
        doNotFuckingChangeRotatingStateThisFrame = false;
        ClearOutlines();
        ClearInputHint();

        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        rayEnd.transform.position = ray.GetPoint(rayLength);

        int playerLayer = LayerMask.GetMask("Player");
        int layerMask = ~playerLayer;

        if (isHoldingItem)
        {
            Item hItem = holdingItem.GetComponent<Item>();
            if (hItem.canBePlacedNow) AddToInputHint("[E] Confrm");
            if (hItem.storable) AddToInputHint("[F] Store");
            if (hItem.rotatable && !inRotationMode) AddToInputHint("[R] Rotate");

            if (Input.GetKeyDown(KeyCode.E) && hItem.canBePlacedNow)
            {
                PIStopHolding();
                doNotFuckingChangeHoldingStateThisFrame = true;
                inRotationMode = false;
                hItem.GetComponent<Item>().ExitRotationMode();
            }

            if (Input.GetKeyDown(KeyCode.F) && hItem.storable)
            {
                PIStoreHoldingItem();
                inRotationMode = false;
                hItem.GetComponent<Item>().ExitRotationMode();
                doNotFuckingChangeHoldingStateThisFrame = true;
            }

            if (Input.GetKeyDown(KeyCode.R) && hItem.rotatable && !inRotationMode)
            {
                hItem.StartRotationMode();
                inRotationMode = true;
                doNotFuckingChangeRotatingStateThisFrame = true;
            }

            if (Input.GetKeyDown(KeyCode.E) && hItem.usable)
            {
                //TODO: use items
            }
        }
        if (inRotationMode)
        {
            AddToInputHint("[R] Exit rotation mode");
            AddToInputHint("[X] Select axis X");
            AddToInputHint("[Y] Select axis Y");
            AddToInputHint("[Z] Select axis Z");

            if (Input.GetKeyDown(KeyCode.R) && !doNotFuckingChangeRotatingStateThisFrame)
            {
                print("yes");
                holdingItem.GetComponent<Item>().ExitRotationMode();
                inRotationMode = false;
            }
             
            if (Input.GetKeyDown(KeyCode.X)) currentRotationAxis = rotationAxis.X;
            if (Input.GetKeyDown(KeyCode.Y)) currentRotationAxis = rotationAxis.Y;
            if (Input.GetKeyDown(KeyCode.Z)) currentRotationAxis = rotationAxis.Z;

            switch (currentRotationAxis)
            {
                case rotationAxis.X:
                    holdingItem.transform.Rotate(Vector3.right * (Input.GetAxis("Mouse ScrollWheel") * rotationStrength));
                    break;
                case rotationAxis.Y:
                    holdingItem.transform.Rotate(Vector3.up * (Input.GetAxis("Mouse ScrollWheel") * rotationStrength));
                    break;
                case rotationAxis.Z:
                    holdingItem.transform.Rotate(Vector3.forward * (Input.GetAxis("Mouse ScrollWheel") * rotationStrength));
                    break;
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
                            if (Input.GetKeyDown(KeyCode.E) && !doNotFuckingChangeHoldingStateThisFrame &&
                                hit.collider.GetComponent<Item>().holdable)
                            {
                                PIStartHolding(hit.collider.gameObject);
                            }
                            else if (Input.GetKeyDown(KeyCode.F) && !doNotFuckingChangeHoldingStateThisFrame &&
                                     hit.collider.GetComponent<Item>().storable)
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
