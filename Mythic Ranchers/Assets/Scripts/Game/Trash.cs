using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trash : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();
        if (inventoryItem.count > 1)
        {            
            inventoryItem.count--;
            inventoryItem.RefreshCount();
        }
        else
        {
            Destroy(dropped);
        }
    }
}
