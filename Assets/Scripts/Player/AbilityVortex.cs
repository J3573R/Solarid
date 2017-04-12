using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVortex : AbilityBase
{
    [SerializeField] private GameObject _blast;

    /// <summary>
    /// Gets the mouseposition and throws the grenade to it
    /// </summary>
    public override void Execute(Vector3 targetPos)
    {
        Vector3 target = targetPos;

        if (target != Vector3.zero)
        {
            target.y = 1;
            Instantiate(_blast, target, Quaternion.Euler(90, 0, 0));
        }

    }
    
}
