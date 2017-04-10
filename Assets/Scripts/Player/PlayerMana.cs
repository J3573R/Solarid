using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour {

    [SerializeField]
    private int _maxMana;
    [SerializeField]
    private float _manaRechargeTickTime;
    [SerializeField]
    private int _PassiveRechargeAmount;
    [SerializeField]
    private float _manaBarFlashDuration;
    [SerializeField]
    private float _manaBarFlashTime;
    
    private int _currentMana;
    private float _rechargeTimer = 0;
    private HudBarController _controller;
    private Image _manaFlashImage;
    private bool _flashManaBar;

    private float _lerpTarget;
    private float _lerpStart;
    private float _lerpTime;
    public bool Initialized;

    public int CurrentMana
    {
        get { return _currentMana; }
        private set { _currentMana = value; }
    }

    // Use this for initialization
    void Start () {
        Init();
    }

    public void Init()
    {
        if (!Initialized)
        {
            _controller = GameObject.Find("HudMana").GetComponent<HudBarController>();
            _manaFlashImage = GameObject.Find("HudNoMana").GetComponent<Image>();
            AddMana(_maxMana);
            _manaFlashImage.CrossFadeAlpha(0, 0, true);
            Initialized = true; 
        }
    }

    // Update is called once per frame
    void Update () {
		if (_currentMana < _maxMana)
        {
            PassiveManaRecharge();
        }

        UpdateManaDisplay();
	}

    /// <summary>
    /// Check if player has enough mana for certain ability
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool HasEnoughMana(int amount)
    {
        if (amount <= _currentMana)
        {
            return true;
        }

        if (!_flashManaBar)
        {
            _flashManaBar = true;
            FlashManaBar(true);
            StartCoroutine(StopFlashing());
        } else
        {
            StopCoroutine(StopFlashing());
            
            StartCoroutine(StopFlashing());
        }

        return false;
    }

    /// <summary>
    /// Timer to stop mana bar flash effect
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopFlashing()
    {
        yield return new WaitForSeconds(_manaBarFlashDuration);
        StopAllCoroutines();
        _flashManaBar = false;
        FlashManaBar(false);

    }

    /// <summary>
    /// Update progress of mana bar
    /// </summary>
    private void UpdateManaDisplay()
    {        
        _controller.Progress = ((float)_currentMana) / ((float)_maxMana);

    }

    /// <summary>
    /// Substract mana from player
    /// </summary>
    /// <param name="amount"></param>
    public void SubStractMana(int amount)
    {
        _controller.SecondaryProgress += ((float)amount) / ((float)_maxMana);
        CurrentMana -= amount;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, _maxMana);
    }

    /// <summary>
    /// Add x amount of mana for player
    /// </summary>
    /// <param name="amount"></param>
    public void AddMana(int amount)
    {
        CurrentMana += amount;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, _maxMana);
    }

    /// <summary>
    /// Passive recharge for mana
    /// </summary>
    private void PassiveManaRecharge ()
    {
        if (_rechargeTimer >= _manaRechargeTickTime)
        {
            _rechargeTimer = 0;
            AddMana(_PassiveRechargeAmount);
        }
        else
            _rechargeTimer += Time.deltaTime;
    }

    /// <summary>
    /// Changes alpha of the mana bar effect, to visible or inbisible
    /// </summary>
    /// <param name="state">true if visible</param>
    internal void FlashManaBar(bool state)
    {
        if (state)
        {
            _manaFlashImage.CrossFadeAlpha(1, _manaBarFlashTime, true);

            if (_flashManaBar)
                StartCoroutine(FlashDelay(false));
        }            
        else
        {
            _manaFlashImage.CrossFadeAlpha(0, _manaBarFlashTime, true);

            if (_flashManaBar)
                StartCoroutine(FlashDelay(true));
        }           
    }

    /// <summary>
    /// Delay before mana bar flash effect fades again
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private IEnumerator FlashDelay(bool state)
    {
        yield return new WaitForSeconds(_manaBarFlashTime);
        FlashManaBar(state);
        
    }
}
