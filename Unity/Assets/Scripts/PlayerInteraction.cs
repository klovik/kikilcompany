using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public Text itemLabelText;
    float rayLength = 3f;
    public KeyCode interactionKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.G;
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
        if (!GetComponent<PlayerMovement>().noclip) itemLabelText.color = Color.white;
        else itemLabelText.color = Color.red;

        inputHintText.text = "";
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetKey(dropKey) && PlayerInventory.inventory[PlayerInventory.activeSlot] != PlayerInventory.Item.None)
        {
            GameObject item = Resources.Load($"Trash/{PlayerInventory.inventory[PlayerInventory.activeSlot].ToString()}") as GameObject;
            GameObject _Item = Instantiate(item, transform.position, new Quaternion(0,0,0,0));
            gameObject.GetComponent<PlayerInventory>().RemoveItemAtIndex(PlayerInventory.activeSlot);
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

                if (Input.GetKeyDown(interactionKey))
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
                if (Input.GetKeyDown(interactionKey) && hit.collider.name == "Terminal")
                {
                    ChangeConsoleState(true);
                }
                if(Input.GetKeyDown(interactionKey) && hit.collider.name == "Lever")
                {
                    GameObject obpc = GameObject.FindGameObjectWithTag("OBPC");
                    obpc.GetComponent<OnBoardPC>().UseLever();
                }
                if(Input.GetKeyDown(interactionKey) && hit.collider.name == "DNS")
                {
                    hit.collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
                itemLabelText.text = hit.collider.gameObject.GetComponent<Label>().text;
            }
            else if(hit.collider.tag == "Selling")
            {
                string labelText = hit.collider.gameObject.GetComponent<Label>().text;
                inputHintText.text += "[E] Sell Item\n";
                int price = 0;
                for(int i = 0; i < hit.collider.transform.childCount; i++)
                {
                    if (hit.collider.transform.GetChild(i).GetComponent<Item>() != null)
                        price += hit.collider.transform.GetChild(i).GetComponent<Item>().price;
                }
                itemLabelText.text = $"{labelText} [{price}]";
                if (Input.GetKeyDown(interactionKey) && PlayerInventory.currentItem != PlayerInventory.Item.None)
                {
                    PlayerInventory.Item activeItem = PlayerInventory.currentItem;
                    gameObject.GetComponent<PlayerInventory>().RemoveItemAtIndex(PlayerInventory.activeSlot);
                    GameObject dropPrefab = Resources.Load<GameObject>($"Trash/{activeItem}");
                    GameObject dropped = Instantiate(dropPrefab);
                    dropped.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    dropped.transform.parent = sellingStand.transform;
                    dropped.transform.localPosition = new Vector3(Random.Range(-0.66f, 0.3f), 1f, Random.Range(-1.7f, 1.7f));
                }
                else if(Input.GetKeyDown(interactionKey) && PlayerInventory.currentItem == PlayerInventory.Item.None)
                {
                    for(int i = 0; i < sellingStand.transform.childCount; i++)
                    {
                        if(sellingStand.transform.GetChild(i).name != "Model")
                        {
                            Destroy(sellingStand.transform.GetChild(i).gameObject);
                        }
                    }
                    GM.money += price;
                    GameObject spp = Instantiate(sellParticlePrefab);
                    spp.transform.parent = sellingStand.transform;
                    spp.transform.localPosition = Vector3.up;
                }
            }
        }
        else
        {
            itemLabelText.text = "";
            inputHintText.text = $"";
        }
        Debug.DrawRay(cam.transform.position, ray.direction * 2, Color.yellow);

        if(GetComponent<PlayerMovement>().noclip)
        {
            itemLabelText.text = "NOCLIP";
        }

    }

    public void ChangeConsoleState(bool state)
    {
        consolePanel.SetActive(state);
        gameObject.GetComponent<PlayerMovement>().ChangePlayerMovementAbility(!state);
    }
}
