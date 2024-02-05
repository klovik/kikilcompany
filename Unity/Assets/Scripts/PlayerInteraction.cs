using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Mirror;

public class PlayerInteraction : NetworkBehaviour
{
    public Camera cam;
    public Text itemLabelText;
    float rayLength = 3f;
    public KeyCode interactionKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.G;

    private void Start()
    {
        //cam = Camera.main;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Input.GetKey(dropKey) && PlayerInventory.inventory[PlayerInventory.activeSlot] != PlayerInventory.Item.None)
            {
                GameObject item = Resources.Load($"Trash/{PlayerInventory.inventory[PlayerInventory.activeSlot].ToString()}") as GameObject;
                GameObject _Item = Instantiate(item, transform.position, new Quaternion(0, 0, 0, 0));
                PlayerInventory.RemoveItemAtIndex(PlayerInventory.activeSlot);
            }

            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (hit.collider.tag == "Item")
                {
                    string itemName = hit.collider.gameObject.GetComponent<Item>().itemName;
                    string itemPrice = hit.collider.gameObject.GetComponent<Item>().price.ToString();
                    itemLabelText.text = $"[{itemPrice}] {itemName}";
                    if (Input.GetKeyDown(interactionKey))
                    {
                        PlayerInventory.Item itemType;
                        itemType = hit.collider.gameObject.GetComponent<Item>().itemType;
                        int res = PlayerInventory.AddItem(itemType);
                        if (res == 0) NetworkServer.Destroy(hit.collider.gameObject);
                    }
                }
                else if (hit.collider.tag == "Interactable")
                {
                    itemLabelText.text = hit.collider.gameObject.GetComponent<Label>().text;
                }
            }
            else
            {
                itemLabelText.text = "";
            }
            Debug.DrawRay(cam.transform.position, ray.direction * 2, Color.yellow);
        }
    }
}
