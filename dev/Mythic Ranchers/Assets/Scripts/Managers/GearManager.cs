using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*******************************************************************************

   Nom du fichier: GearManager.cs
   
   Contexte: Cette classe sert à gérer l'équipement du joueur
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class GearManager : MonoBehaviour
{
    public static GearManager instance;

    public Item[] gear;
    public InventorySlot[] gearSlots;
    public GameObject inventoryItemPrefab;
    public Image[] iconList;
    public TextMeshProUGUI staminaValue, strengthValue, intellectValue, agilityValue, armorValue, hasteValue, leechValue;
    public StatText staminaText, strengthText, intellectText, agilityText, armorText, hasteText, leechText;
    private PlayerUnit player;

    public void SetPlayerUnit(PlayerUnit playerUnit)
    {
        this.player = playerUnit;
        UpdateGear();
    }

    private void Awake()
    {
        instance = this;
    }

    public void UpdateGear()
    {
        Dictionary<string, int> tempInitialStats = new Dictionary<string, int>(player.InitialStats);
        player.Stats = tempInitialStats;
        
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
        staminaValue.SetText(player.Stats["stamina"].ToString());
        staminaText.tooltipText = ("Increases HP by " + (player.Stats["stamina"] * 10f + 100f).ToString());
        strengthValue.SetText(player.Stats["strength"].ToString());
        strengthText.tooltipText = ("Increases basic attack power by " + (player.Stats["strength"] * 1.5f + 100f).ToString());
        intellectValue.SetText(player.Stats["intellect"].ToString());
        intellectText.tooltipText = ("Increases mana by " + (player.Stats["intellect"] * 10f + 100f).ToString());
        agilityValue.SetText(player.Stats["agility"].ToString());
        agilityText.tooltipText = ("Increases agility by " + player.Stats["agility"].ToString());
        armorValue.SetText(player.Stats["armor"].ToString());
        armorText.tooltipText = ("Increases armor by " + player.Stats["armor"].ToString());
        hasteValue.SetText(player.Stats["haste"].ToString());
        hasteText.tooltipText = ("Increases haste by " + player.Stats["haste"].ToString());
        leechValue.SetText(player.Stats["leech"].ToString());
        leechText.tooltipText = ("Increases leech by " + player.Stats["leech"].ToString());
    }
}
