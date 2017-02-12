using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour {

    private Player _player;
    private AbilityBlink _blink;
    private AbilityGrenade _grenade;
    private AbilityBase _currentAbility;
    private float _abilityIndex;


    // Use this for initialization
    void Start()
    {
        _player = GetComponent<Player>();
        _blink = GetComponent<AbilityBlink>();
        _grenade = GetComponent<AbilityGrenade>();
        _currentAbility = _grenade;
    }

    /// <summary>
    /// Enum for determing abilities. Index number used in scrolling ability selection
    /// </summary>
    public enum Ability
    {
        Blink = 0,
        Grenade = 1,
        SomeRandomAbility = 2
    }

    /// <summary>
    /// Draws the targeting icon
    /// </summary>
    public void Target()
    {
        //TODO: Draw the targeting icon somehow
    }

    /// <summary>
    /// Executes the current ability
    /// </summary>
    public void Execute()
    {
        _currentAbility.Execute();
    }

    /// <summary>
    /// Sets the current ability in use
    /// </summary>
    /// <param name="tmp">Ability to set</param>
    public void SetAbility(Ability tmp)
    {
        if (tmp == Ability.Blink)
            _currentAbility = _blink;
        if (tmp == Ability.Grenade)
            _currentAbility = _grenade;
    }

    //TODO: Weapon selection via "next/last weapon"

}
