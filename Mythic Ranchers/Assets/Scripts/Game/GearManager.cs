using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GearManager : MonoBehaviour
{

    public static GearManager instance { get; set; }

    public Item[] gear;
    public InventorySlot[] gearSlots;
    public GameObject inventoryItemPrefab;
    public Image[] iconList;
    [SerializeField]
    public TextMeshProUGUI stamina, strength, intellect, agility, armor, haste, leech;

    private PlayerUnit player;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGear();
    }

    public void SetPlayerUnit(PlayerUnit playerUnit)
    {
        this.player = playerUnit;
    }

    public void UpdateGear()
    {
        Dictionary<string, int> tempInitialStats = new Dictionary<string, int>(PlayerUnit.instance.InitialStats);
        PlayerUnit.instance.Stats = tempInitialStats;
        
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
        UpdateStatsUI();
    }

    public void UpdateStats(Item item)
    {
        player.Stats["stamina"] += item.stamina;
        player.Stats["strength"] += item.strength;
        player.Stats["intellect"] += item.intellect;
        player.Stats["agility"] += item.agility;
        player.Stats["armor"] += item.armor;
        player.Stats["haste"] += item.haste;
        player.Stats["leech"] += item.leech;
        
    }

    public void UpdateStatsUI()
    {
        stamina.SetText(player.Stats["stamina"].ToString());
        strength.SetText(player.Stats["strength"].ToString());
        intellect.SetText(player.Stats["intellect"].ToString());
        agility.SetText(player.Stats["agility"].ToString());
        armor.SetText(player.Stats["armor"].ToString());
        haste.SetText(player.Stats["haste"].ToString());
        leech.SetText(player.Stats["leech"].ToString());
    }

}
