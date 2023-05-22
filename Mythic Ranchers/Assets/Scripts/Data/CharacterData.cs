using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterData 
{
    private readonly Dictionary<string, float> BERZERKER_MULTIPLIERS = new Dictionary<string, float>()
    {
        {"staminaMultiplier", 1.8f },
        {"strengthMultiplier", 2f },
        {"intellectMultiplier", 0.3f },
        {"agilityMultiplier", 1.1f },
        {"armorMultiplier", 1.7f },
        {"hasteMultiplier", 1f },
        {"leechMultiplier", 1.3f }
    };

    private readonly Dictionary<string, float> MAGE_MULTIPLIERS = new Dictionary<string, float>()
    {
        {"staminaMultiplier", 1.2f },
        {"strengthMultiplier", 0.2f },
        {"intellectMultiplier", 2f },
        {"agilityMultiplier", 0.7f },
        {"armorMultiplier", 0.6f },
        {"hasteMultiplier", 1f },
        {"leechMultiplier", 1f }
    };

    private readonly Dictionary<string, float> NECROMANCER_MULTIPLIERS = new Dictionary<string, float>()
    {
        {"staminaMultiplier", 1f },
        {"strengthMultiplier", 0.2f },
        {"intellectMultiplier", 2f },
        {"agilityMultiplier", 0.5f },
        {"armorMultiplier", 0.8f },
        {"hasteMultiplier", 1f },
        {"leechMultiplier", 1.8f }
    };

    private string name;
    private string username;
    private int level;
    private int experiencPoints;
    private string className;
    private int currentKey;
    private List<EquipmentData> equipmentList;
    private string talents;
    private Dictionary<string, int> stats;

    public CharacterData(string name, string username, int level, int experience_points, string className, int current_key, List<EquipmentData> equipmentList, string talents)
    {
        this.name = name;
        this.username = username;
        this.level = level;
        this.experiencPoints = experience_points;
        this.className = className;
        this.currentKey = current_key;
        this.equipmentList = equipmentList;
        this.talents = talents;
        this.stats = new Dictionary<string, int>();

        CalculateStats();
    }

    public string Name { get => name; set => name = value; }
    public string Username { get => username; set => username = value; }
    public int Level { get => level; set => level = value; }
    public int Experience_points { get => experiencPoints; set => experiencPoints = value; }
    public string ClassName { get => className; set => className = value; }
    public int Current_key { get => currentKey; set => currentKey = value; }
    public List<EquipmentData> EquipmentList { get => equipmentList; set => equipmentList = value; }
    public string Talents { get => talents; set => talents = value; }
    public Dictionary<string, int> Stats { get => stats; set => stats = value; }

    private void CalculateStats()
    {
        Dictionary<string, float> assignedDict = null;
        Dictionary<string, int> equipmentStats = GetEquipmentStats();  //to be implemented

        switch (ClassName)
        {
            case ("Berzerker"):
                assignedDict = BERZERKER_MULTIPLIERS;
                break;
            case ("Mage"):
                assignedDict = MAGE_MULTIPLIERS;
                break;
            case ("Necromancer"):
                assignedDict = NECROMANCER_MULTIPLIERS;
                break;
        }

        Stats["stamina"] = (int) Math.Round(Level * assignedDict["staminaMultiplier"]) + 1;
        Stats["strength"] = (int)Math.Round(Level * assignedDict["strengthMultiplier"]) + 1;
        Stats["intellect"] = (int)Math.Round(Level * assignedDict["intellectMultiplier"]) + 1;
        Stats["agility"] = (int)Math.Round(Level * assignedDict["agilityMultiplier"]) + 1;
        Stats["armor"] = (int)Math.Round(Level * assignedDict["armorMultiplier"]) + 1;
        Stats["haste"] = (int)Math.Round(Level * assignedDict["hasteMultiplier"]) + 1;
        Stats["leech"] = (int)Math.Round(Level * assignedDict["leechMultiplier"]) +1;
    }


    private Dictionary<string, int> GetEquipmentStats()
    {
        return null;
    }

}
