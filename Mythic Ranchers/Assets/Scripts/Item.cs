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
    //public ActionType actionType;
    //public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable;

    [Header("Both")]
    public Sprite image;
}

public enum ItemType
{
    Equipement,
    Key,
    Potion
}

//public enum ActionType
//{

//}
