using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour {

    private Player _player;
    private AbilityBlink _blink;
    private AbilityGrenade _grenade;
    private AbilityBase _currentAbility;


    // Use this for initialization
    void Start()
    {
        _player = GetComponent<Player>();
        _blink = GetComponent<AbilityBlink>();
        _grenade = GetComponent<AbilityGrenade>();
        _currentAbility = _grenade;
    }


    public void Target()
    {
        
    }

    public void Execute()
    {
        _currentAbility.Execute();
    }
}
