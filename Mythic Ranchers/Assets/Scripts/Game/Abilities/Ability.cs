using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Abilities")]
public class Ability : ScriptableObject
{
    [Header("Gameplay")]
    public AbilityType type;
    public string abilityName;
    public float range;
    public float damage;
    public float radius;
    public float cooldown;
    public float cost;
    public float duration;
    public float tick;

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
