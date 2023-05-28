using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/*******************************************************************************

   Nom du fichier: InventorySlot.cs
   
   Contexte: Cette classe représente un slot dans l'inventaire ou dans l'action bar du joueur
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public TMP_Text hotkeyText;
    public SlotType slotType;
    public GearSlot gearSlot;

    public Image image;
    public Color selectedColor, notSelectedColor, noRessourceColor, noRessourceSelectedColor;
    public bool selected;

    public enum SlotType
    {
        Inventory,
        ActionBar,
        TalentTree,
        CharacterGear
    }

    private void Awake()
    {
        Deselect();
        selected = false;
    }

    public void Select()
    {
        image.color = selectedColor;
        selected = true;
    }

    public void Deselect()
    {
        if (image)
        {
            image.color = notSelectedColor;
            selected = false;
        }
    }

    public void NoRessourceColor()
    {
        if (image)
        {
            if (!selected)
            {
                image.color = noRessourceColor;
            }
            else if (selected)
            {
                image.color = noRessourceSelectedColor;
            }
        }
    }

    public void EnoughRessourceColor()
    {
        if (image)
        {
            if (!selected)
            {
                image.color = notSelectedColor;
            }
            else if (selected)
            {
                image.color = selectedColor;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();

        if (this.slotType == SlotType.TalentTree)
        {
            Debug.Log("Can't put anything in the talent tree");
        }
        else if (this.slotType == SlotType.Inventory && inventoryItem.ability != null)
        {
            Debug.Log("Can't put abilities in the inventory");
        }
        else if (this.slotType == SlotType.CharacterGear)
        {

            //cheeky appel de instance player

            if (InventoryManager.instance.player.ArmorType == inventoryItem.item.ArmorType || inventoryItem.item.ArmorType == ArmorType.Misc)
            {
                if (inventoryItem.item.gearSlot == this.gearSlot)
                {
                    inventoryItem.parentAfterDrag = transform;
                }
                else
                {
                    Debug.Log("Wrong gear slot");
                }
            }
            else
            {
                Debug.Log("Your character cannot equip this armor type");
            }
        }
        else if (transform.childCount == 1)
        {
            inventoryItem.parentAfterDrag = transform;
        }
        
        // stacking items that are the same but in two different slots -- to be implemented
        //else if (transform.childCount == (2) && inventoryItem.item == transform.GetChild(1).item && inventoryItem.count < InventoryManager.instance.maxItemStacks)
        //{
        //    inventoryItem.parentAfterDrag = transform;
        //    inventoryItem.count++;
        //    inventoryItem.RefreshCount();
        //}
    }
}
