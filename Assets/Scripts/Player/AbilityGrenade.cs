using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGrenade : AbilityBase {

    private Player _player;
    [SerializeField]
    private GameObject _grenade;
    private GrenadeScript _grenadeScript;

    public override void Execute()
    {
        _grenadeScript.targetPosition = _player.input.GetMousePosition();
        _grenadeScript.Throw();
    }

    // Use this for initialization
    void Start () {
        _grenade = Instantiate(_grenade, transform.position, Quaternion.identity);
        _grenadeScript = _grenade.GetComponent<GrenadeScript>();
	}

}
