using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData 
{
    private string name;
    private string username;
    private int level;
    private int experiencPoints;
    private string className;
    private int currentKey;
    private List<EquipmentData> equipmentList;
    private string talents;

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
    }

    public string Name { get => name; set => name = value; }
    public string Username { get => username; set => username = value; }
    public int Level { get => level; set => level = value; }
    public int Experience_points { get => experiencPoints; set => experiencPoints = value; }
    public string ClassName { get => className; set => className = value; }
    public int Current_key { get => currentKey; set => currentKey = value; }
    public List<EquipmentData> EquipmentList { get => equipmentList; set => equipmentList = value; }
    public string Talents { get => talents; set => talents = value; }
}
