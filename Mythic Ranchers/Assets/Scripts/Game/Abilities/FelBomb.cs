using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FelBomb : AbilityAoeTargeted
{
    [SerializeField]
    private Ability ability;

    private void Start()
    {
        base.Ability = ability;
        base.Start();
    }
}
