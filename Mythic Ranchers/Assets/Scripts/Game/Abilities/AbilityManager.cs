using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    public Ability[] abilities;
    public InventorySlot[] talentTreeSlots;
    public GameObject inventoryItemPrefab;
    public GameObject[] abilitiesPrefab;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            FindSlot(abilities[i]);
        }
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

    public bool UseAbility(Vector3 playerPos)
    {
        InventorySlot slot = InventoryManager.instance.inventorySlots[InventoryManager.instance.selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (!itemInSlot.ability)
        {
            return false;
        }
        if (!itemInSlot.IsCastable())
        {
            return false;
        }
        int counter = 0;
        foreach (Ability ability in abilities)
        {
            string abilityName = ability.abilityName;
            if (abilityName == itemInSlot.ability.abilityName)
            {
                Instantiate(abilitiesPrefab[counter], playerPos, Quaternion.identity);
                return true;
            }
            counter++;
        }
        return false;
    }
}

public static class ArrayExtensions
{
    public static bool ContainsCustom<InventorySlot>(this InventorySlot[] slots, InventorySlot slot)
    {
        return slots.Contains(slot);
    }
}
