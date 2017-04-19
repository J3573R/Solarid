using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health {

    private HudBarController _controller;
    private int _originalHP;
    private Player _player;
    public bool Initialized;
    public bool Invulnerable;

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
            _player = GetComponent<Player>();
            Invulnerable = false;
            Initialized = true;
        }
    }

    public override bool TakeDamage(int damage)
    {
        if (!Invulnerable)
        {
            _player.Interact = false;
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

        return IsDead();        
    }

    /// <summary>
    /// Kills the player
    /// </summary>
    private void Die()
    {
        GameStateManager.Instance.GameLoop.Player.gameObject.GetComponent<Player>().Dead = true;
        GameStateManager.Instance.GameLoop.Player.Die();
        //TODO: Implement dying
    }
}
