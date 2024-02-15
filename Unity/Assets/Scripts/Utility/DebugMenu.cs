using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    private bool isDebugMenuOpened = false;
    private GameObject itemList;
    public GameObject itemPrefab;

    private void Start()
    {
        itemList = transform.GetChild(1).GetChild(0).gameObject;
        UpdateItemList();
    }
    public void SwitchPanel(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(i == index) transform.GetChild(i).gameObject.SetActive(true);
            else transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyBindings.debugToggle)) ChangeDebugMenuState(!isDebugMenuOpened);
    }
    private void ChangeDebugMenuState(bool state)
    {
        GetComponent<Animator>().Play(state ? "Open" : "Close");
        isDebugMenuOpened = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void UpdateItemList()
    {
        DestroyAllItemsList();
        GameObject[] items = Resources.LoadAll<GameObject>("Trash");
        for (int i = 0; i < items.Length; i++)
        {
            Sprite spr = Resources.Load<Sprite>($"Sprites/Items/{items[i].name}");
            if (spr != null)
            {
                GameObject item = Instantiate(itemPrefab, itemList.transform);
                item.GetComponent<Image>().sprite = spr;
                int index = i;
                item.GetComponent<Button>().onClick.AddListener(delegate { CreateItemByString(items[index].name); }); // Используем локальную переменную вместо i
            }
            else
            {
                
            }
        }
    }

    public void CreateItemByString(string item)
    {
        GameObject itemGO = Resources.Load<GameObject>($"Trash/{item}");
        GameObject itemInstance = Instantiate(itemGO);
        PlayerInteraction.PIStartHolding(itemInstance);
    }

    private void DestroyAllItemsList()
    {
        for (int i = 0; i < itemList.transform.childCount; i++)
        {
            Destroy(itemList.transform.GetChild(i).gameObject);
        }
    }

    public void Open() => ChangeDebugMenuState(true);
    public void Close() => ChangeDebugMenuState(false);
}
