﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sort of "master" component for all player related, has links to all other components
/// </summary>
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    [HideInInspector] public InputController Input;
    [HideInInspector] public AbilityController AbilityController;
    [HideInInspector] public Health Health;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerAnimation Animation;
    [HideInInspector] public Gun Gun;
    [HideInInspector] public PlayerMana Mana;
    [HideInInspector] public List<GameObject> Clones;
    public bool ShootingEnabled = true;
    public Slider HealthBar;
    
    public bool Dead {get; private set; }    

    void Awake()
    {
        Globals.Player = gameObject;
        Gun = GetComponentInChildren<Gun>();
        Input = GetComponent<InputController>();
        AbilityController = GetComponent<AbilityController>();
        Movement = GetComponent<PlayerMovement>();
        Animation = GetComponent<PlayerAnimation>();        
        Health = GetComponent<Health>();
        Mana = GetComponent<PlayerMana>();
        HealthBar.maxValue = Health.CurrentHealth;
        Dead = false;
    }

    /// <summary>
    /// Tries to shoot
    /// </summary>
    public void Shoot()
    {
        if (ShootingEnabled && !Movement.Casting)
        {
            Gun.Shoot();
        }
    }

    /// <summary>
    /// Damages the player
    /// </summary>
    /// <param name="damage">How much damage</param>
    /// <returns>True if player died, else false</returns>
    public bool TakeDamage(int damage)
    {

        Globals.Interact = false;
        if (Health.TakeDamage(damage))
        {
            Die();
            HealthBar.value = Health.CurrentHealth;
            return true;
        }

        HealthBar.value = Health.CurrentHealth;
        return false;
    }


    /// <summary>
    /// Kills the player
    /// </summary>
    private void Die()
    {
        Dead = true;        
        //TODO: Implement dying
    }
}
