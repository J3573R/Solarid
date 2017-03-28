using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sort of "master" component for all player related, has links to all other components
/// </summary>
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [HideInInspector] public InputController Input;
    [HideInInspector] public AbilityController AbilityController;
    [HideInInspector] public Health Health;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerAnimation Animation;
    [HideInInspector] public Gun Gun;
    [HideInInspector] public PlayerMana Mana;
    public bool ShootingEnabled = true;
    
    public bool Dead { get; set; }    

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

}
