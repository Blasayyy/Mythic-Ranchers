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

    [Header("UI")]
    public Sprite image;
}

public enum AbilityType
{
    Projectile,
    Frontal,
    AoeTargetted,
    AoeStandard,
    Passive
}
