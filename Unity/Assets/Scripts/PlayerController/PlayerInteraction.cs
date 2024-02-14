using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float normalRayLength = 3.5f;
    public float rayLength = 3.5f;
    public float rotationStrength = 4f;
    [SerializeField] private float rayAdjustCoefficient = 0.3f;
    public Text inputHintText;
    public Transform rayEnd;
    public static List<Outline> Outlines = new List<Outline>();
    private static bool isHoldingItem = false;
    private static GameObject holdingItem = null;
    private bool inRotationMode = false;
    private rotationAxis currentRotationAxis = rotationAxis.None;
    private bool hasItemInHand = false;

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

        hasItemInHand = PlayerInventory.handSlot != PlayerInventory.ItemId.None;

        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        rayEnd.transform.position = ray.GetPoint(rayLength);

        int playerLayer = LayerMask.GetMask("Player");
        int layerMask = ~playerLayer;

        if (isHoldingItem)
        {
            Item hItem = holdingItem.GetComponent<Item>();
            AddToInputHint("[SHFT MWHL] Adjust distance");
            if (hItem.canBePlacedNow) AddToInputHint("[E] Confirm");
            if (hItem.storable) AddToInputHint("[F] Store");
            if (hItem.rotatable && !inRotationMode) AddToInputHint("[R] Rotate");

            if (Input.GetAxis("Mouse ScrollWheel") < 0f && Input.GetKey(KeyCode.LeftShift)) rayLength -= rayAdjustCoefficient;
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && Input.GetKey(KeyCode.LeftShift)) rayLength += rayAdjustCoefficient;

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
            AddToInputHint("[R] Stop rotating");
            AddToInputHint("[SHFT+R] Reset rotation");
            AddToInputHint("[X] Select axis X");
            AddToInputHint("[Y] Select axis Y");
            AddToInputHint("[Z] Select axis Z");

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.R))
            {
                holdingItem.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.R) && !doNotFuckingChangeRotatingStateThisFrame)
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

        if (hasItemInHand)
        {
            AddToInputHint("[Q] Store item");
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayerInventory.AddItem(PlayerInventory.handSlot);
                PlayerInventory.handSlot = PlayerInventory.ItemId.None;
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
                            if (item.holdable && !hasItemInHand) AddToInputHint("[E] Pickup");
                            if (item.storable && !hasItemInHand) AddToInputHint("[F] Store");
                            if (GameManager.developer && !hasItemInHand) AddToInputHint("[C] Copy");
                            
                            if (!hasItemInHand)
                            {
                                outline.enabled = true;
                                Outlines.Add(outline);
                            }
                            
                            if (Input.GetKeyDown(KeyCode.E) && !doNotFuckingChangeHoldingStateThisFrame &&
                                hit.collider.GetComponent<Item>().holdable && !hasItemInHand)
                            {
                                PIStartHolding(hit.collider.gameObject);
                            }
                            else if (Input.GetKeyDown(KeyCode.F) && !doNotFuckingChangeHoldingStateThisFrame &&
                                     hit.collider.GetComponent<Item>().storable && !hasItemInHand)
                            {
                                item.Store();
                            }
                            else if (Input.GetKeyDown(KeyCode.C) && GameManager.developer && !hasItemInHand)
                            {
                                string itemName = hit.collider.GetComponent<Item>().itemIdType.ToString();
                                Quaternion copyingRotation = hit.collider.transform.rotation;
                                GameObject newItemPrefab = Resources.Load<GameObject>($"Trash/{itemName}");
                                if (newItemPrefab != null)
                                {
                                    GameObject newItem = Instantiate(newItemPrefab);
                                    newItem.transform.rotation = copyingRotation;
                                    PIStartHolding(newItem);
                                }
                                else
                                {
                                    print("Can't copy");
                                }
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
        PlayerInteraction PI = GameObject.Find("Player").GetComponent<PlayerInteraction>();
        PI.rayLength = PI.normalRayLength;
    }

    public static void PIStoreHoldingItem()
    {
        holdingItem.GetComponent<Item>().Store();
        isHoldingItem = false;
        PlayerInteraction PI = GameObject.Find("Player").GetComponent<PlayerInteraction>();
        PI.rayLength = PI.normalRayLength;
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
