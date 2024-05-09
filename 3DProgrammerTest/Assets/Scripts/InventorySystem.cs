using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    [Header("Inventory list")]

    public List<int> weapons = new List<int>();
    public List<int> instruments = new List<int>();

    [Header("Base prefab")]
    [SerializeField] GameObject itemUI;

    [Header("Inventory UI")]

    [SerializeField] GameObject inventory;
    [SerializeField] GameObject instrumentsPanel;
    [HideInInspector] public GameObject[] instrumentsSlots;
    [SerializeField] GameObject weaponsPanel;
    [HideInInspector] public GameObject[] weaponsSlots;

    private bool isInventoryOpen = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //Lock the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        instrumentsSlots = new GameObject[instrumentsPanel.transform.childCount];

        for (int i = 0; i < instrumentsSlots.Length; i++)
        {
            instrumentsSlots[i] = instrumentsPanel.transform.GetChild(i).gameObject;
        }

        weaponsSlots = new GameObject[weaponsPanel.transform.childCount];

        for (int i = 0; i < weaponsSlots.Length; i++)
        {
            weaponsSlots[i] = weaponsPanel.transform.GetChild(i).gameObject;
        }

        InitializeInventory(PlayerController.instance.weaponsSO, weapons, weaponsSlots);
        InitializeInventory(PlayerController.instance.instrumentsSO, instruments, instrumentsSlots);
    }

    private void InitializeInventory(ItemsSO inventoryName, List<int> listName, GameObject[] inventoryPanel)
    {
        for (int i = 0; i < listName.Count; i++)
        {
            GameObject uiObject = Instantiate(itemUI);
            uiObject.name = inventoryName.data[i].Name;
            uiObject.gameObject.GetComponent<Image>().sprite = inventoryName.data[i].Sprite;
            uiObject.transform.SetParent(inventoryPanel[i].transform, false);
        }
    }

    public void PutOnInventory(ItemsSO inventoryName, GameObject item, List<int> listName, GameObject[] inventoryPanel)
    {
        //Put the pick up item to the inventory
        for (int i = 0; i < inventoryName.data.Count; i++)
        {
            if (item.name == inventoryName.data[i].Name)
            {
                listName.Add(inventoryName.data[i].ID);

                GameObject uiObject = Instantiate(itemUI);
                uiObject.name = item.name;
                uiObject.gameObject.GetComponent<Image>().sprite = inventoryName.data[i].Sprite;

                for (int j = 0; j < inventoryPanel.Length; j++)
                {
                    if (inventoryPanel[j].transform.childCount == 0)
                    {
                        uiObject.transform.SetParent(inventoryPanel[j].transform, false);
                        return;
                    }
                }
            }
        }
    }

    public void OpenInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        Cursor.visible = isInventoryOpen;

        if (isInventoryOpen)
        {
            inventory.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            inventory.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public bool CheckIfItemHave(ItemsSO inventoryName, List<int> listName)
    {
        for (int i = 0; i < listName.Count; i++)
        {
            if (inventoryName.data[i].ID == listName[i])
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveFromInventory(ItemsSO inventoryName, GameObject item, List<int> listName, GameObject[] inventoryPanel)
    {
        for (int i = 0; i < listName.Count; i++)
        {
            if (item.name == inventoryName.data[i].Name)
            {
                listName.RemoveAt(i);

                for (int j = 0; j < inventoryPanel.Length; j++)
                {
                    if (inventoryPanel[j].transform.childCount > 0)
                    {
                        Destroy(inventoryPanel[j].transform.GetChild(0).gameObject);
                    }
                }
            }
        }
    }
}
