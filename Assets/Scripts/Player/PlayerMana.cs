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

    private Player _player;
    private int _currentMana;
    private float _rechargeTimer = 0;
    private HudBarController _controller;
    private Image _manabar;
    private Image _manaFlashImage;
    private bool _flashManaBar;
    private Color _manaBarOriginalColor;

    private float _lerpTarget;
    private float _lerpStart;
    private float _lerpTime;
    

    public int CurrentMana
    {
        get { return _currentMana; }
        private set { _currentMana = value; }
    }

    // Use this for initialization
    void Start () {
        _player = GetComponent<Player>();
        _controller = GameObject.Find("HudMana").GetComponent<HudBarController>();
        _manabar = GameObject.Find("HudMana").GetComponent<Image>();
        _manaFlashImage = GameObject.Find("HudNoMana").GetComponent<Image>();
        _manaBarOriginalColor = _manabar.color;
        AddMana(_maxMana);

        _manaFlashImage.CrossFadeAlpha(0, 0, true);
    }
	
	// Update is called once per frame
	void Update () {
		if (_currentMana < _maxMana)
        {
            PassiveManaRecharge();
        }

        UpdateManaDisplay();
	}

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

    private IEnumerator StopFlashing()
    {
        yield return new WaitForSeconds(_manaBarFlashDuration);
        StopAllCoroutines();
        _flashManaBar = false;
        FlashManaBar(false);

    }

    private void UpdateManaDisplay()
    {        
        _controller.Progress = ((float)_currentMana) / ((float)_maxMana);

    }

    public void SubStractMana(int amount)
    {
        _controller.SecondaryProgress += ((float)amount) / ((float)_maxMana);
        CurrentMana -= amount;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, _maxMana);
    }

    public void AddMana(int amount)
    {
        CurrentMana += amount;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, _maxMana);
    }

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

    internal void FlashManaBar(bool state)
    {
        Debug.Log("flashing");
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

    private IEnumerator FlashDelay(bool state)
    {
        yield return new WaitForSeconds(_manaBarFlashTime);
        FlashManaBar(state);
        
    }
}
