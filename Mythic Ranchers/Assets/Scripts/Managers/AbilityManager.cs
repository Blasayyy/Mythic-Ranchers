using UnityEngine;
using System.Linq;

/*******************************************************************************

   Nom du fichier: AbilityManager.cs
   
   Contexte: Cette classe gère les abilities des joueurs, pour les instancier et
             les donner aux bonnes classes au début
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    public Ability[] abilities;
    public InventorySlot[] talentTreeSlots;
    public GameObject inventoryItemPrefab;
    public GameObject[] abilitiesPrefab;
    public PlayerUnit player;

    public void SetPlayerUnit(PlayerUnit playerUnit)
    {
        this.player = playerUnit;
        foreach (Ability ability in abilities)
        {
            if (ability.className == player.ClassName)
                FindSlot(ability);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void FindSlot(Ability ability)
    {
        foreach (InventorySlot slot in talentTreeSlots)
        {
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
                Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cursorWorldPosition.z = 0;
                MythicGameManagerMultiplayer.Instance.RequestUseAbility(counter, playerPos, cursorWorldPosition);
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
