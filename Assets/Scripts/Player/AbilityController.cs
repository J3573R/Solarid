using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour {

    private Player _player;    
    private AbilityBlink _blink;
    private AbilityVortex _vortex;
    private AbilityClone _clone;
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
        _vortex = GetComponent<AbilityVortex>();
        _clone = GetComponent<AbilityClone>();
        _currentAbility = _blink;
        _cooldownDisplay = GameObject.Find("CoolDown").GetComponent<Text>();
        _rangeCheck = FindObjectOfType<RangeCheck>();

        if (SaveSystem.Instance.SaveData != null)
            AbilityArray = SaveSystem.Instance.SaveData.GetAbilityArray();
        else
        {
            GameStateManager.Instance.SetupSaveSystem();
            AbilityArray = SaveSystem.Instance.SaveData.GetAbilityArray();
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
        Clone = 2
    }

    /// <summary>
    /// Enables or disables ability in the controller, can be used in runtime
    /// </summary>
    /// <param name="ability">the ability</param>
    /// <param name="state">true or false</param>
    public void EnableOrDisableAbility(Ability ability, bool state)
    {
        AbilityArray[ability] = state;
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

        if (AbilityArray[Ability.Clone])
        {
            _clone.enabled = true;
            tmpIndex += 1;
        }
        else
            _clone.enabled = false;        

        if (tmpIndex == 0)
        {
            _allAbilitiesDisabled = true;
            _cooldownDisplay.enabled = false;
        }
        else
        {
            _allAbilitiesDisabled = false;
            _cooldownDisplay.enabled = true;
        }
            

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
        bool notExecuted = true;
        if (!_allAbilitiesDisabled)
        {
            if (_player.Movement.Casting && !_player.Movement.Shooting && GetCurrentCooldownProgress() <= 0)
            {
                if (_player.Mana.HasEnoughMana(_currentAbility.ManaCost))
                {
                    Vector3 targetPosition = _player.Input.GetMouseGroundPosition();
                    bool inRange = true;

                    if (GetRange() == 0)
                    {
                        _currentAbility.Execute(targetPosition);
                        notExecuted = false;
                    }
                    if (_rangeCheck.GetDistance() > GetRange())
                        inRange = false;

                    if (inRange && targetPosition != Vector3.zero && notExecuted)
                    {
                        _currentAbility.Execute(targetPosition);
                        notExecuted = false;
                    }

                    targetPosition = _rangeCheck.GetMaxRangePosition(GetRange());

                    if (!inRange && targetPosition != Vector3.zero && notExecuted)
                    {
                        _currentAbility.Execute(targetPosition);
                        notExecuted = false;
                    }

                    targetPosition = _rangeCheck.GetNextSuitablePosition(GetRange());

                    if (targetPosition != Vector3.zero && notExecuted)
                    {
                        _currentAbility.Execute(targetPosition);
                        notExecuted = false;
                    }
                    if (notExecuted)
                    {
                        _currentAbility.Execute(new Vector3(transform.position.x, _rangeCheck.transform.position.y, transform.position.z));
                    }

                    _player.Mana.SubStractMana(_currentAbility.ManaCost);
                }
            }
        }        
    }

    /// <summary>
    /// Returns remaining cooldown of the current ability
    /// </summary>
    /// <returns></returns>
    public float GetCurrentCooldownProgress()
    {
        if (!_allAbilitiesDisabled)
        {
            float cd = Mathf.Clamp(_currentAbility.GetRemainingCooldown(), 0, 1);

            return cd;
        }
            
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
        if (tmp == Ability.Clone && _clone.enabled)
        {
            _currentAbility = _clone;
            _abilityIndex = 2;
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
            SetAbility(Ability.Clone);
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
        string name = "";

        if (_currentAbility == _blink)
            name = "Blink: ";
        if (_currentAbility == _vortex)
            name = "Vortex: ";
        if (_currentAbility == _clone)
            name = "Clone: ";

        if (tmp <= 0)
        {
            _cooldownDisplay.text = name + "Ready";
        }
        else
        {
            _cooldownDisplay.text = name + tmp.ToString();
        }
    }
}
