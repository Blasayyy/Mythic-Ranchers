using UnityEngine;

/*******************************************************************************

   Nom du fichier: Item.cs
   
   Contexte: Cette classe représente un patron pour la création d'un item
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    public string itemName;
    public ItemType type;
    public GearSlot gearSlot;
    public ArmorType ArmorType;
    public float RestoresHealth;
    public float RestoresMana;
    public float RestoresEnergy;

    [Header("Only UI")]
    public bool stackable;
    public bool useable;

    [Header("Both")]
    public Sprite image;
    [TextArea(3, 10)]
    public string tooltip;
    public int stamina, strength, intellect, agility, armor, haste, leech;
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