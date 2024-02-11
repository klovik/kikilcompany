using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Fragsurf.Movement;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public Text itemLabelText;
    float rayLength = 3f;
    public GameObject consolePanel;
    public GameObject sellingStand;
    public GameObject sellParticlePrefab;
    public GameManager GM;

    public Text inputHintText;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        cam = Camera.main;
    }

    void Update()
    {
        inputHintText.text = "";
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetKey(KeyBindings.drop) && PlayerInventory.inventory[PlayerInventory.activeSlot] != PlayerInventory.Item.None)
        {
            DropCurrentItem();
        }

        //input hint text update
        if (PlayerInventory.currentItem != PlayerInventory.Item.None)
        {
            inputHintText.text += $"[G] Drop\n";
        }
        switch (PlayerInventory.currentItem)
        {
            case PlayerInventory.Item.None:
            case PlayerInventory.Item.Pioneer:
            case PlayerInventory.Item.ECord:
            case PlayerInventory.Item.TV:
            case PlayerInventory.Item.ViperRAM:
            case PlayerInventory.Item.HPPrinter:
                break;
            case PlayerInventory.Item.ElectroEbator:
                inputHintText.text += "[LMB] Use\n";
                break;
        }

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if(hit.collider.tag == "Item")
            {
                inputHintText.text += "[E] Pickup\n";
                string itemName = hit.collider.gameObject.GetComponent<Item>().itemName;
                string itemPrice = hit.collider.gameObject.GetComponent<Item>().price.ToString();
                if(hit.collider.GetComponent<Item>().priceless) itemLabelText.text = $"{itemName}";
                else itemLabelText.text = $"[{itemPrice}] {itemName}";

                if (Input.GetKeyDown(KeyBindings.use))
                {
                    PlayerInventory.Item itemType;
                    itemType = hit.collider.gameObject.GetComponent<Item>().itemType;
                    int res = PlayerInventory.AddItem(itemType);
                    if (res == 0) Destroy(hit.collider.gameObject);
                }
            }
            else if(hit.collider.tag == "Interactable")
            {
                inputHintText.text += "[E] Interact\n";
                if(Input.GetKeyDown(KeyBindings.use))
                {
                    //action with interactables
                    switch (hit.collider.name)
                    {
                        case "Terminal":
                            ConsoleHandler.Open(); break;
                        case "Lever":
                            GameObject obpc = GameObject.FindGameObjectWithTag("OBPC");
                            obpc.GetComponent<OnBoardPC>().UseLever();
                            break;
                        case "DNS":
                            hit.collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                            break;
                    }
                }
                itemLabelText.text = hit.collider.gameObject.GetComponent<Label>().text;
            }
            else if(hit.collider.tag == "Selling")
            {
                string labelText = hit.collider.gameObject.GetComponent<Label>().text;
                inputHintText.text += "[E] Sell Item\n";
                //update labelText with total price of selling items;
                itemLabelText.text = $"{labelText} [{GetTotalSellingPrice()}]";

                //if holding something
                if (Input.GetKeyDown(KeyBindings.use) && PlayerInventory.currentItem != PlayerInventory.Item.None)
                {
                    SellCurrentItem();
                }
                //if holding nothing
                else if(Input.GetKeyDown(KeyBindings.use) && PlayerInventory.currentItem == PlayerInventory.Item.None)
                {
                    SellEverything();
                }
            }
        }
        else
        {
            itemLabelText.text = "";
            inputHintText.text = $"";
        }
        Debug.DrawRay(cam.transform.position, ray.direction * 2, Color.yellow);
    }

    private void SellCurrentItem()
    {
        PlayerInventory.Item activeItem = PlayerInventory.currentItem;
        gameObject.GetComponent<PlayerInventory>().RemoveItemAtIndex(PlayerInventory.activeSlot);
        GameObject dropPrefab = Resources.Load<GameObject>($"Trash/{activeItem}");
        GameObject dropped = Instantiate(dropPrefab);
        dropped.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        dropped.transform.parent = sellingStand.transform;
        dropped.transform.localPosition = new Vector3(Random.Range(-0.66f, 0.3f), 1f, Random.Range(-1.7f, 1.7f));
    }
    private void SellEverything()
    {
        for (int i = 0; i < sellingStand.transform.childCount; i++)
        {
            if (sellingStand.transform.GetChild(i).name != "Model")
            {
                Destroy(sellingStand.transform.GetChild(i).gameObject);
            }
        }
        GameManager.money += GetTotalSellingPrice();
        GameObject spp = Instantiate(sellParticlePrefab);
        spp.transform.parent = sellingStand.transform;
        spp.transform.localPosition = Vector3.up;
    }
    private int GetTotalSellingPrice()
    {
        int price = 0;
        for (int i = 0; i < sellingStand.transform.childCount; i++)
        {
            if (sellingStand.transform.GetChild(i).GetComponent<Item>() != null)
                price += sellingStand.transform.GetChild(i).GetComponent<Item>().price;
        }
        return price;
    }

    private void DropCurrentItem()
    {
        GameObject item = Resources.Load<GameObject>($"Trash/{PlayerInventory.inventory[PlayerInventory.activeSlot]}");
        GameObject _Item = Instantiate(item, transform.position, new Quaternion(0, 0, 0, 0));
        gameObject.GetComponent<PlayerInventory>().RemoveItemAtIndex(PlayerInventory.activeSlot);
    }
}
