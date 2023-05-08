using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TalentTreeManager : MonoBehaviour
{
    public static TalentTreeManager instance;

    public Ability[] abilities;
    public InventorySlot[] talentTreeSlots;
    public GameObject inventoryItemPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void FindSlot(Ability ability)
    {
        for (int i = 0; i < talentTreeSlots.Length; i++)
        {
            InventorySlot slot = talentTreeSlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                AddAbility(ability, slot);
                break;
            }
        }
    }

    public void AddAbility(Ability ability, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeAbility(ability);
    }

    public void AddDuplicate(Ability ability, Transform slot)
    {
        if (ArrayExtensions.ContainsCustom<InventorySlot>(talentTreeSlots, slot.GetComponent<InventorySlot>()))
        {
            GameObject newItemGo = Instantiate(inventoryItemPrefab, slot);
            InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
            inventoryItem.InitializeAbility(ability);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            FindSlot(abilities[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class ArrayExtensions
{
    public static bool ContainsCustom<InventorySlot>(this InventorySlot[] slots, InventorySlot slot)
    {
        return slots.Contains(slot);
    }
}
