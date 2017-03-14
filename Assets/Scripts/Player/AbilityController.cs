using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour {

    private Player _player;    
    private AbilityBlink _blink;
    private AbilityBlast _vortex;
    private AbilityConfusion _confusion;
    private AbilityLightning _lightning;
    private AbilityBase _currentAbility;
    private float _abilityIndex;
    private Text _cooldownDisplay;
    private RangeCheck _rangeCheck;

    public float CastDelayInSeconds;


    // Use this for initialization
    void Start()
    {
        _player = GetComponent<Player>();
        _blink = GetComponent<AbilityBlink>();
        _vortex = GetComponent<AbilityBlast>();
        _confusion = GetComponent<AbilityConfusion>();
        _lightning = GetComponent<AbilityLightning>();
        _currentAbility = _blink;
        GameObject tmp = GameObject.Find("CoolDown");
        _cooldownDisplay = tmp.GetComponent<Text>();
        _rangeCheck = FindObjectOfType<RangeCheck>();
    }

    /// <summary>
    /// Enum for determing abilities. Index number used in scrolling ability selection
    /// </summary>
    public enum Ability
    {
        Blink = 0,
        Grenade = 1,
        Confusion = 2,
        Lightning = 3
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
        if (_player.Movement.Casting && !_player.Movement.Shooting && GetCurrentCooldown() <= 0)
        {
            _currentAbility.Execute();         
        }
            
    }

    public float GetCurrentCooldown()
    {
        return _currentAbility.GetRemainingCooldown();
    }

    public float GetRange()
    {
        return _currentAbility.GetRange();
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
            _currentAbility = _vortex;
            _abilityIndex = 1;
        }
        if (tmp == Ability.Confusion)
        {
            _currentAbility = _confusion;
            _abilityIndex = 2;
        }
        if (tmp == Ability.Lightning)
        {
            _currentAbility = _lightning;
            _abilityIndex =32;
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

    public void DrawRange(bool draw)
    {
        /*if (draw)
            _rangeCheck.DrawRange(GetRange(), true);
        else
            _rangeCheck.DrawRange(1f, false);
       */
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
