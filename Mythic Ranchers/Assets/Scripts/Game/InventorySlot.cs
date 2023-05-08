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

    public Image image;
    public Color selectedColor, notSelectedColor;

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
        if (transform.childCount == 1)
        {
            inventoryItem.parentAfterDrag = transform;
        }
        //else if (transform.GetChild(2) == inventoryItem)
        //{
        //    inventoryItem.parentAfterDrag = transform;
        //}
    }
}
