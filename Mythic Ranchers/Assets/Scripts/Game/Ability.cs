using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Abilities")]
public class Ability : ScriptableObject
{
    [Header("Gameplay")]
    public TileBase tile;
    public AbilityType type;
    public float range;
    public float damage;
    public float aoeRange;
    public float ressourceCost;
    public float cooldown;
    public float cost;

    [Header("UI")]
    public Sprite image;

    [Header("Both")]
    public string tooltip;
}

public enum AbilityType
{
    Projectile,
    Frontal,
    AoeTargetted,
    AoeStandard,
    Passive
}
