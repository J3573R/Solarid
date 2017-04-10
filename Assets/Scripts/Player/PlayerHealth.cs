﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health {

    private HudBarController _controller;
    private int _originalHP;
    public bool Initialized;

    // Use this for initialization
    void Start () {
        Init();
	}

    public void Init()
    {
        if (!Initialized)
        {
            _originalHP = _health;
            _controller = GameObject.Find("HudHealth").GetComponent<HudBarController>();
            Initialized = true;
        }
    }

    public override bool TakeDamage(int damage)
    {
        Globals.Interact = false;
        if (!IsDead())
        {            
            float secondary = ((float)damage) / ((float)_originalHP);
            float progress = ((float)_health) / ((float)_originalHP);

            CurrentHealth -= damage;

            if (secondary > progress)
                secondary = progress;

            progress = ((float)_health) / ((float)_originalHP);
            _controller.SecondaryProgress += secondary;
            _controller.Progress = progress;
        }

        if (IsDead())
        {
            Die();
        }
        return IsDead();

        
    }

    /// <summary>
    /// Kills the player
    /// </summary>
    private void Die()
    {
        Globals.Player.GetComponent<Player>().Dead = true;
        
        //TODO: Implement dying
    }

    // Update is called once per frame
    void Update () {
		
	}
}
