using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour {

    private Player _player;
    private AbilityBlink _blink;
    private AbilityBase _currentAbility;


    // Use this for initialization
    void Start()
    {
        _player = GetComponent<Player>();
        _blink = GetComponent<AbilityBlink>();
        _currentAbility = _blink;
    }


    public void Execute()
    {
        _currentAbility.Execute();
    }
}
