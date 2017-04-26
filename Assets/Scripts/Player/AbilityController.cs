using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour {

    [SerializeField]
    private float _maxCooldown;

    private float _currentCooldown;
    private Player _player;    
    private AbilityBlink _blink;
    private AbilityVortex _vortex;
    private AbilityClone _clone;
    private AbilityBase _currentAbility;
    private float _abilityIndex;
    private RangeCheck _rangeCheck;    
    private int _maxAbilityIndex;
    private HudController _hudController;
    private ParticleSystem _blinkCharge;
    private ParticleSystem _vortexCharge;
    private ParticleSystem _cloneCharge;

    public ParticleSystem _currentCharge;
    public bool _allAbilitiesDisabled;
    public float CastDelayInSeconds;
    public Dictionary<Ability, bool> AbilityArray;
    public bool Initialized;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (!Initialized)
        {
            _player = GetComponent<Player>();
            _blink = GetComponent<AbilityBlink>();
            _vortex = GetComponent<AbilityVortex>();
            _clone = GetComponent<AbilityClone>();
            _currentAbility = _blink;
            _hudController = FindObjectOfType<HudController>();
            _hudController.init();
            _rangeCheck = FindObjectOfType<RangeCheck>();
            _blinkCharge = GameObject.Find("BlinkCharge").GetComponent<ParticleSystem>();
            _vortexCharge = GameObject.Find("VortexCharge").GetComponent<ParticleSystem>();
            _cloneCharge = GameObject.Find("CloneCharge").GetComponent<ParticleSystem>();
            _blinkCharge.Stop();
            _vortexCharge.Stop();
            _cloneCharge.Stop();

            _currentCharge = _blinkCharge;
            
            Initialized = true;
        }
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

    private void Update()
    {
        if (!GameStateManager.Instance.GameLoop.Paused)
        {
            if (_currentCooldown > 0)
            {
                _currentCooldown -= Time.deltaTime;
            }
        }        
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
            _hudController.EnableSmallIcon(Ability.Blink);
            tmpIndex += 1;
        }
        else
            _blink.enabled = false;
        if (AbilityArray[Ability.Vortex])
        {
            _hudController.EnableSmallIcon(Ability.Vortex);
            _vortex.enabled = true;
            tmpIndex += 1;
        }
        else
            _vortex.enabled = false;

        if (AbilityArray[Ability.Clone])
        {
            _hudController.EnableSmallIcon(Ability.Clone);
            _clone.enabled = true;
            tmpIndex += 1;
        }
        else
            _clone.enabled = false;        

        if (tmpIndex == 0)
        {
            _hudController.AllAbilitesDisabled(true);
            _allAbilitiesDisabled = true;
        }
        else
        {
            _allAbilitiesDisabled = false;
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
                    _currentCooldown = _maxCooldown;
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
            float cd = Mathf.Clamp(_currentCooldown, 0, 1);

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
        bool chargeState = _currentCharge.isPlaying;

        if (_currentCharge.isPlaying)
            _currentCharge.Stop();     

        if (tmp == Ability.Blink && _blink.enabled)
        {
            _currentAbility = _blink;
            _abilityIndex = 0;
            _currentCharge = _blinkCharge;
        }            
        if (tmp == Ability.Vortex && _vortex.enabled)
        {
            _currentAbility = _vortex;
            _abilityIndex = 1;
            _currentCharge = _vortexCharge;
        }
        if (tmp == Ability.Clone && _clone.enabled)
        {
            _currentAbility = _clone;
            _abilityIndex = 2;
            _currentCharge = _cloneCharge;
        }

        if (chargeState)
        {
            _currentCharge.Play();
        }
        _rangeCheck.UpdateRange(_currentAbility.GetRange());
        _hudController.ChangeImage(tmp);
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

}
