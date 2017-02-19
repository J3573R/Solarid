using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGrenade : AbilityBase {

    private Player _player;
    [SerializeField]
    private GameObject _grenade;
    public float angle;
    private GrenadeScript _grenadeScript;

    /// <summary>
    /// Gets the mouseposition and throws the grenade to it
    /// </summary>
    public override void Execute()
    {
        if (CoolDownRemaining <= 0)
        {
            Vector3 target = _player.Input.GetMousePosition();

            if (target != Vector3.zero)
            {
                _grenadeScript.targetPosition = target;
                _grenadeScript.ResetPosition(transform);
                _grenadeScript.Throw();
                CoolDownRemaining = CoolDown;
            }
        }

        //TODO: Cooldown/mana stuff needed, for all abilities
    }

    

    // Use this for initialization
    void Start () {
        _grenade = Instantiate(_grenade, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        _grenadeScript = _grenade.GetComponent<GrenadeScript>();
        _player = GetComponent<Player>();
        _grenadeScript.angle = angle;
	}    

}
