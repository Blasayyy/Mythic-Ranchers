using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearManager : MonoBehaviour
{

    public static GearManager instance;

    public Item[] gear;
    public InventorySlot[] gearSlots;
    public GameObject inventoryItemPrefab;
    public Image[] iconList;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGearUI();
    }

    public void UpdateGearUI()
    {
        for (int i = 0; i < gearSlots.Length; i++)
        {
            InventorySlot slot = gearSlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                slot.image.sprite = iconList[7].sprite;
            }
            else
            {
                slot.image.sprite = iconList[i].sprite;
            }
        }
    }
}
