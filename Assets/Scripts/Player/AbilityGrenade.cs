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

    public override void Execute()
    {
        Vector3 target = _player.input.GetMousePosition();

        if (target != Vector3.zero)
        {
            _grenadeScript.targetPosition = target;
            _grenadeScript.ResetPosition(transform);
            _grenadeScript.Throw();
        }
    }

    // Use this for initialization
    void Start () {
        _grenade = Instantiate(_grenade, transform.position, Quaternion.identity);
        _grenadeScript = _grenade.GetComponent<GrenadeScript>();
        _player = GetComponent<Player>();
        _grenadeScript.angle = angle;
	}

    
}
