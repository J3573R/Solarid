using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityConfusion : AbilityBase
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
        Debug.Log("Execture");
        Vector3 target = _player.Input.GetMouseGroundPosition();

        var test = Instantiate(_blast, transform.position, Quaternion.identity);
        CoolDownRemaining = CoolDown;
        //TODO: Cooldown/mana stuff needed, for all abilities
    }
    
}
