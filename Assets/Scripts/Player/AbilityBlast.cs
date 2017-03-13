using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBlast : AbilityBase
{

    private Player _player;
    [SerializeField]
    private GameObject _blast;

    // Use this for initialization
    void Start()
    {
        _player = GetComponent<Player>();
    }

    /// <summary>
    /// Gets the mouseposition and throws the grenade to it
    /// </summary>
    public override void Execute()
    {
        Vector3 target = _player.Input.GetMouseGroundPosition();

        if (target != Vector3.zero)
        {
            target.y = 1;           
            Instantiate(_blast, target, Quaternion.Euler(90, 0, 0));
            CoolDownRemaining = CoolDown;
        }


        //TODO: Cooldown/mana stuff needed, for all abilities
    }
    
}
