using UnityEngine;
using UnityEngine.EventSystems;

/*******************************************************************************

   Nom du fichier: Trash.cs
   
   Contexte: Cette classe représente l'endroit ou le joueur peut jeter des items
             non voulus
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

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
