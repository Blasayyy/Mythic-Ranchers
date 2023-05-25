using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ArcaneNova : AbilityAoeStandard
{
    [SerializeField]
    private Ability ability;

    private void Start()
    {
        base.Ability = ability;
        base.Start();
    }
}
