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
    
        // not working
        PlayerUnit.instance.Stats = PlayerUnit.instance.InitialStats;
        for (int i = 0; i < gearSlots.Length; i++)
        {
            InventorySlot slot = gearSlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                slot.image.sprite = iconList[7].sprite;
                UpdateStats(itemInSlot.item);
            }
            else
            {
                slot.image.sprite = iconList[i].sprite;
            }
        }
    }

    public void UpdateStats(Item item)
    {
        PlayerUnit player = PlayerUnit.instance;
        player.Stats["stamina"] += item.stamina;
        player.Stats["strength"] += item.strength;
        player.Stats["intellect"] += item.intellect;
        player.Stats["agility"] += item.agility;
        player.Stats["armor"] += item.armor;
        player.Stats["haste"] += item.haste;
        player.Stats["leech"] += item.leech;
    }
}
