using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    public Ability[] abilities;
    public InventorySlot[] talentTreeSlots;

    public GameObject inventoryItemPrefab;
    public GameObject felBombPrefab;
    public GameObject voidboltPrefab;

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

    public bool UseAbility(Vector3 target, Vector3 playerPos)
    {
        InventorySlot slot = InventoryManager.instance.inventorySlots[InventoryManager.instance.selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot.ability)
        {
            if (itemInSlot.UseSpell())
            {
                if (itemInSlot.ability.type == AbilityType.AoeTargetted)
                {
                    Instantiate(felBombPrefab, target, Quaternion.identity);
                    return true;
                }
                else if (itemInSlot.ability.type == AbilityType.Projectile)
                {
                    Instantiate(voidboltPrefab, playerPos, Quaternion.identity);
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
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
