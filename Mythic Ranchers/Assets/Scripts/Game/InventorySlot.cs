using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    public TMP_Text hotkeyText;
    public SlotType slotType;

    public Image image;
    public Color selectedColor, notSelectedColor;

    public enum SlotType
    {
        Inventory,
        ActionBar,
        TalentTree,
        CharacterEquipement
    }

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        if (image)
        {
            image.color = notSelectedColor;
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
        else if (transform.childCount == 1)
        {
            inventoryItem.parentAfterDrag = transform;
        }
        //else if (transform.childCount == (2) && inventoryItem.item == transform.GetChild(1).item && inventoryItem.count < InventoryManager.instance.maxItemStacks)
        //{
        //    inventoryItem.parentAfterDrag = transform;
        //    inventoryItem.count++;
        //    inventoryItem.RefreshCount();
        //}


    }
}
