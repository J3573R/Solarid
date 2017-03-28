using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health {

    private HudBarController _controller;
    private int _originalHP;

	// Use this for initialization
	void Start () {
        _originalHP = _health;
        _controller = GameObject.Find("HudHealth").GetComponent<HudBarController>();
	}

    public override bool TakeDamage(int damage)
    {
        Globals.Interact = false;
        if (!IsDead())
        {
            _health -= damage;
            _controller.SecondaryProgress += ((float)damage) / ((float)_originalHP);

            if (_health < damage)
            {
                _health = 0;
            }
            _controller.Progress = ((float)_health) / ((float)_originalHP);
            Debug.Log(_controller.Progress);
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
