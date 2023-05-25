using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public int maxItemStacks = 4;
    public InventorySlot[] inventorySlots;
    public InventorySlot[] actionBarSlots;
    public GameObject inventoryItemPrefab;
    public GameObject felBombPrefab;
    public GameObject voidboltPrefab;
    public string hotkey;
    public int selectedSlot = -1;

    public PlayerUnit player;

    public void SetPlayerUnit(PlayerUnit playerUnit)
    {
        this.player = playerUnit;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
        //string hotkey;
        for (int i = 0; i <= 10; i++)
        {
            switch (i)
            {
                case 0:
                    hotkey = "1";
                    break;
                case 1:
                    hotkey = "2";
                    break;
                case 2:
                    hotkey = "3";
                    break;
                case 3:
                    hotkey = "4";
                    break;
                case 4:
                    hotkey = "5";
                    break;
                case 5:
                    hotkey = "Q";
                    break;
                case 6:
                    hotkey = "E";
                    break;
                case 7:
                    hotkey = "R";
                    break;
                case 8:
                    hotkey = "F";
                    break;
                case 9:
                    hotkey = "C";
                    break;
                case 10:
                    hotkey = "V";
                    break;
            }
            inventorySlots[i].hotkeyText.text = hotkey;
        }
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 6)
            {
                ChangeSelectedSlot(number - 1);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeSelectedSlot(5);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeSelectedSlot(6);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                ChangeSelectedSlot(7);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeSelectedSlot(8);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeSelectedSlot(9);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                ChangeSelectedSlot(10);
            }
        }

        foreach (InventorySlot slot in actionBarSlots)
        {
            Ability ability = slot.GetComponentInChildren<InventoryItem>()?.ability;
            if (ability != null)
                if (player.CurrentRessource < ability.cost)
                {
                    slot.NoRessourceColor();
                }
                else
                {
                    slot.EnoughRessourceColor();
                }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool AddItem(Item item)
    {
        // Check if any slot has the same item with count lower than max
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxItemStacks && itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // Find any empty slot
        for (int i =0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use)
            {
                player.GetHealed(item.RestoresHealth);
                player.GainRessource(item.RestoresMana);

                Debug.Log("mana pot consumed + " + item.RestoresMana + " mana");


                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }

            return item;
        }
        return null;
    }
}
