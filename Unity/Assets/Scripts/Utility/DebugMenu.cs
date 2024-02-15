using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    private bool isDebugMenuOpened = false;
    private GameObject itemList;
    private GameObject sceneList;
    public GameObject itemPrefab;
    public GameObject scenePrefab;
    public Text freePlacementButtonText;

    private void Start()
    {
        itemList = transform.GetChild(1).GetChild(0).gameObject;
        sceneList = transform.GetChild(2).GetChild(0).gameObject;
        UpdateItemList();
        UpdateScenesList();
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
        GameObject[] items = Resources.LoadAll<GameObject>("Trash");
        for (int i = 0; i < items.Length; i++)
        {
            Sprite spr = Resources.Load<Sprite>($"Sprites/Items/{items[i].name}");
            if (spr != null)
            {
                GameObject item = Instantiate(itemPrefab, itemList.transform);
                item.GetComponent<Image>().sprite = spr;
                int index = i;
                item.GetComponent<Button>().onClick.AddListener(delegate { CreateItemByString(items[index].name); });
            }
        }
    }

    private void UpdateScenesList()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        for (int i = 0; i < scenes.Length; i++)
        {
            GameObject sceneButton = Instantiate(scenePrefab, sceneList.transform);
            int index = i;
            sceneButton.GetComponent<Button>().onClick.AddListener(delegate { LoadSceneByIndex(index); });
            sceneButton.GetComponent<Text>().text = scenes[index].path.Split('/')[2];
        }
        
    }

    private void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void CreateItemByString(string item)
    {
        GameObject itemGO = Resources.Load<GameObject>($"Trash/{item}");
        GameObject itemInstance = Instantiate(itemGO);
        PlayerInteraction.PIStartHolding(itemInstance);
    }

    public void ToggleFreePlacement()
    {
        PlayerInteraction.freePlacement = !PlayerInteraction.freePlacement;
        freePlacementButtonText.color = PlayerInteraction.freePlacement ? new Color(0, 255, 0) : new Color(255, 255, 255);
    }
    public void Open() => ChangeDebugMenuState(true);
    public void Close() => ChangeDebugMenuState(false);
}
