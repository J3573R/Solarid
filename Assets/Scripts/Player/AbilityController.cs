using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour {

    private Player _player;
    private PlayerDash _dash;
    private AbilityBase _currentAbility;


    // Use this for initialization
    void Start()
    {
        _player = GetComponent<Player>();
        _dash = GetComponent<PlayerDash>();
        _currentAbility = _dash;
    }


    public void Execute()
    {
        Debug.Log("HAJAAH");
        _currentAbility.Execute();
    }
}
