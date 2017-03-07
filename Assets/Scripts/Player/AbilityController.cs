using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour {

    private Player _player;    
    private AbilityBlink _blink;
    private AbilityGrenade _grenade;
    private AbilityBase _currentAbility;
    private float _abilityIndex;
    private Text _cooldownDisplay;

    public float CastDelayInSeconds;


    // Use this for initialization
    void Start()
    {
        _player = GetComponent<Player>();
        _blink = GetComponent<AbilityBlink>();
        _grenade = GetComponent<AbilityGrenade>();
        _currentAbility = _blink;
        GameObject tmp = GameObject.Find("CoolDown");
        _cooldownDisplay = tmp.GetComponent<Text>();
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
        //Debug.Log("trytoshoot");
        if (_player.Movement.Casting && !_player.Movement.Shooting && GetCurrentCooldown() <= 0)
        {
            StartCoroutine(CastDelay());
            _player.Animation.CastOnce = true;
        }
            
    }

    public float GetCurrentCooldown()
    {
        return _currentAbility.GetRemainingCooldown();
    }

    private IEnumerator CastDelay()
    {
        yield return new WaitForSeconds(CastDelayInSeconds);
        _currentAbility.Execute();
    }

    /// <summary>
    /// Sets the current ability in use
    /// </summary>
    /// <param name="tmp">Ability to set</param>
    public void SetAbility(Ability tmp)
    {
        if (tmp == Ability.Blink)
        {
            _currentAbility = _blink;
            _abilityIndex = 0;
        }            
        if (tmp == Ability.Grenade)
        {
            _currentAbility = _grenade;
            _abilityIndex = 1;
        }            
    }

    /// <summary>
    /// Scrolls current weapon index
    /// </summary>
    /// <param name="tmp"></param>
    public void ScrollWeapon(int tmp)
    {
        //Debug.Log(_abilityIndex);
        _abilityIndex += tmp;
        //Debug.Log(_abilityIndex);

        if (_abilityIndex < 0)
            _abilityIndex = 1;
        else if (_abilityIndex > 1)
            _abilityIndex = 0;

        if (_abilityIndex == 0)
            SetAbility(Ability.Blink);
        if (_abilityIndex == 1)
            SetAbility(Ability.Grenade);
        //Debug.Log("TRUEINDEX = " + _abilityIndex);
    }


    private void Update()
    {
        DisplayCooldown();
    }

    /// <summary>
    /// Gets current abilitys cooldown and displays it. "Ready" if no cooldown remaining
    /// </summary>
    private void DisplayCooldown()
    {
        int tmp = (int)_currentAbility.GetRemainingCooldown();

        if (tmp <= 0)
        {
            _cooldownDisplay.text = "Ready";
        }
        else
        {
            _cooldownDisplay.text = "Cooldown: " + tmp.ToString();
        }
    }
}
