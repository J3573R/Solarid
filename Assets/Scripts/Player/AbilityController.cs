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
    private int _maxAbilityIndex;

    public bool _allAbilitiesDisabled;
    public float CastDelayInSeconds;
    public Dictionary<Ability, bool> AbilityArray;

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

        if (SaveSystem.Instance.SaveData != null)
            AbilityArray = SaveSystem.Instance.SaveData.GetAbilityArray();
        else
        {
            GameStateManager.Instance.SetupSaveSystem();
            AbilityArray = SaveSystem.Instance.SaveData.GetAbilityArray();
        }  

        if (SaveSystem.Instance.SaveData.GetAbilityArray() == null) {
            Debug.Log("NULL SAATANA");
        }

        SetupAbilites();

        
    }

    /// <summary>
    /// Enum for determing abilities. Index number used in scrolling ability selection
    /// </summary>
    public enum Ability
    {
        Blink = 0,
        Vortex = 1,
        Confusion = 2,
        Lightning = 3
    }

    /// <summary>
    /// Enables or disables ability in the controller, can be used in runtime
    /// </summary>
    /// <param name="ability">the ability</param>
    /// <param name="state">true or false</param>
    public void EnableOrDisableAbility(Ability ability, bool state)
    {
        AbilityArray[ability] = state;
        foreach (KeyValuePair<Ability, bool> pair in AbilityArray)
        {
            Debug.Log(pair.Key + " = " + pair.Value);
        }
        SetupAbilites();
    }

    /// <summary>
    /// Goes through the Dictionary and enables or disables abilities accordingly. 
    /// Also counts new max index value for scrolling weapons selection
    /// </summary>
    private void SetupAbilites()
    {
        int tmpIndex = 0;

        if (AbilityArray[Ability.Blink])
        {
            _blink.enabled = true;
            tmpIndex += 1;
        }
        else
            _blink.enabled = false;
        if (AbilityArray[Ability.Vortex])
        {
            _vortex.enabled = true;
            tmpIndex += 1;
        }
        else
            _vortex.enabled = false;

        if (AbilityArray[Ability.Confusion])
        {
            _confusion.enabled = true;
            tmpIndex += 1;
        }
        else
            _confusion.enabled = false;

        if (AbilityArray[Ability.Lightning])
        {
            _lightning.enabled = true;
            tmpIndex += 1;
        }
        else
            _lightning.enabled = false;

        if (tmpIndex == 0)
            _allAbilitiesDisabled = true;
        else
            _allAbilitiesDisabled = false;

        if (tmpIndex> 0)
        {
            tmpIndex -= 1;
        }

        _maxAbilityIndex = tmpIndex;
    }

    /// <summary>
    /// Executes the current ability
    /// </summary>
    public void Execute()
    {
        if (!_allAbilitiesDisabled)
        {
            if (_player.Movement.Casting && !_player.Movement.Shooting && GetCurrentCooldown() <= 0)
            {
                if (GetRange() == 0)
                {
                    _currentAbility.Execute();
                }
                else
                {
                    if (_rangeCheck.GetDistance() <= GetRange())
                        _currentAbility.Execute();
                }
            }
        }        
    }

    /// <summary>
    /// Returns remaining cooldown of the current ability
    /// </summary>
    /// <returns></returns>
    public float GetCurrentCooldown()
    {
        if (!_allAbilitiesDisabled)
            return _currentAbility.GetRemainingCooldown();
        else
            return 0;
    }

    /// <summary>
    /// Returns the Max range of the current ability
    /// </summary>
    /// <returns></returns>
    public float GetRange()
    {
        if (!_allAbilitiesDisabled)
            return _currentAbility.GetRange();
        else
            return 0;
    }

    /// <summary>
    /// Sets the current ability in use
    /// </summary>
    /// <param name="tmp">Ability to set</param>
    public void SetAbility(Ability tmp)
    {
        if (tmp == Ability.Blink && _blink.enabled)
        {
            _currentAbility = _blink;
            _abilityIndex = 0;
        }            
        if (tmp == Ability.Vortex && _vortex.enabled)
        {
            _currentAbility = _vortex;
            _abilityIndex = 1;
        }
        if (tmp == Ability.Confusion && _confusion.enabled)
        {
            _currentAbility = _confusion;
            _abilityIndex = 2;
        }
        if (tmp == Ability.Lightning && _lightning.enabled)
        {
            _currentAbility = _lightning;
            _abilityIndex = 3;
        }
    }

    /// <summary>
    /// Scrolls current weapon index and sets the correct ability
    /// </summary>
    /// <param name="tmp"></param>
    public void ScrollWeapon(int tmp)
    {
        _abilityIndex += tmp;
        
        if (_abilityIndex < 0)
            _abilityIndex = _maxAbilityIndex;
        else if (_abilityIndex > _maxAbilityIndex)
            _abilityIndex = 0;

        if (_abilityIndex == 0)
            SetAbility(Ability.Blink);
        if (_abilityIndex == 1)
            SetAbility(Ability.Vortex);
        if (_abilityIndex == 2)
            SetAbility(Ability.Confusion);
        if (_abilityIndex == 3)
            SetAbility(Ability.Lightning);
    }

    /// <summary>
    /// If true, tells RangeCheck to draw the range indicator. If false, stops drawing
    /// </summary>
    /// <param name="draw"></param>
    public void DrawRange(bool draw)
    {
        if (draw)
            _rangeCheck.DrawRange(GetRange(), true);
        else
            _rangeCheck.DrawRange(1f, false);       
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
