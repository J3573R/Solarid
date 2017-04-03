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
    private float _manaBarFlashTime;

    private Player _player;
    private int _currentMana;
    private float _rechargeTimer = 0;
    private HudBarController _controller;
    private Image _manabar;
    private bool _flashManaBar;
    private Color _manaBarOriginalColor;
    

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
        _manaBarOriginalColor = _manabar.color;
        AddMana(_maxMana);        
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

        _flashManaBar = true;
        StartCoroutine(StopFlashing());
        return false;
    }

    private IEnumerator StopFlashing()
    {
        yield return new WaitForSeconds(_manaBarFlashTime);
        _flashManaBar = false;
        _manabar.color = _manaBarOriginalColor;

    }

    private void UpdateManaDisplay()
    {        
        _controller.Progress = ((float)_currentMana) / ((float)_maxMana);

        if (_flashManaBar)
            FlashManaBar();
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

    internal void FlashManaBar()
    {
        Debug.Log("flashing");
        _manabar.color = Color.Lerp(_manaBarOriginalColor, Color.red, Time.deltaTime);

    }
}
