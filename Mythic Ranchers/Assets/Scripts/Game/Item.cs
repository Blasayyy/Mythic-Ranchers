using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    public TileBase tile;
    public ItemType type;
    public GearSlot gearSlot;
    public ArmorType ArmorType;
    public float RestoresHealth;

    [Header("Only UI")]
    public bool stackable;

    [Header("Both")]
    public Sprite image;
    public string tooltip;
}

public enum ItemType
{
    Gear,
    Key,
    Potion
}

public enum GearSlot
{
    Head,
    Hands,
    Weapon,
    Neck,
    Chest,
    Feet,
    OffHand,
    None
}

public enum ArmorType
{
    Cloth,
    Leather,
    Mail,
    Misc,
    None
}
