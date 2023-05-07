using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{

    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;
    public void PickupItems(int id)
    {
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if (result == true)
        {
            Debug.Log("Item added");
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    public void GetSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(false);
        if (receivedItem != null)
        {
            Debug.Log("Received item: " + receivedItem);
        } 
        else
        {
            Debug.Log("No item received");
        }
    }   
    
    public void UseSelectedItem()
    {
        Item usedItem = inventoryManager.GetSelectedItem(true);
        if (usedItem != null)
        {
            Debug.Log("Used item: " + usedItem);
        } 
        else
        {
            Debug.Log("No item used");
        }
    }

}
