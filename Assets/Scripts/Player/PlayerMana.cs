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

    private Player _player;
    public Text ManaText;
    private int _currentMana;
    private float _rechargeTimer = 0;
    

    public int CurrentMana
    {
        get { return _currentMana; }
        private set { _currentMana = value; }
    }

    // Use this for initialization
    void Start () {
        _player = GetComponent<Player>();
        ManaText = GameObject.Find("ManaText").GetComponent<Text>();
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

    private void UpdateManaDisplay()
    {

        string tmp = (CurrentMana / 10).ToString();
        ManaText.text = "Mana: " + tmp;
    }

    public void SubStractMana(int amount)
    {
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
}
