using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityConfusion : AbilityBase
{
    [SerializeField]
    private GameObject _blast;

    /// <summary>
    /// Gets the mouseposition and throws the grenade to it
    /// </summary>
    public override void Execute(Vector3 targetPos)
    {
        CoolDownRemaining = CoolDown;
        //TODO: Cooldown/mana stuff needed, for all abilities
    }
    
}
