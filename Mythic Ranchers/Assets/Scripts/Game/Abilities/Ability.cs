using UnityEngine;

/*******************************************************************************

   Nom du fichier: Ability.cs
   
   Contexte: Cette classe représente un patron pour la création d'une ability
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

[CreateAssetMenu(menuName = "Scriptable object/Abilities")]
public class Ability : ScriptableObject
{
    [Header("Gameplay")]
    public AbilityType type;
    public string abilityName;
    public float range;
    public float potency;
    public float radius;
    public float cooldown;
    public float cost;
    public float duration;
    public float tick;
    public bool helpful;
    public bool slow;
    public float slowDuration;
    public float slowAmount;
    public string className;

    [Header("UI")]
    public Sprite image;

    [Header("Both")]
    [TextArea(3, 10)]
    public string tooltip;
}

public enum AbilityType
{
    Projectile,
    Frontal,
    AoeTargeted,
    AoeStandard,
    Passive
}
