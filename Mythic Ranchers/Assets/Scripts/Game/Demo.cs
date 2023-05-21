using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Demo : MonoBehaviour
{

    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;
    [SerializeField]
    private Transform playerPrefab;
    public Item[] testGear;


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

    public void SpawnTestGear()
    {
        for (int i=0; i < testGear.Length; i++)
        {
            bool result = inventoryManager.AddItem(testGear[i]);
            if (result == true)
            {
                Debug.Log("Item added");
            }
            else
            {
                Debug.Log("Inventory is full");
            }
        }
    }

}
