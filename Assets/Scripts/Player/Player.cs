using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    [HideInInspector] public InputController Input;
    [HideInInspector] public AbilityController AbilityController;
    [HideInInspector] public Health Health;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerAnimation Animation;
    [HideInInspector] public Gun Gun;
    public bool ShootingEnabled = true;
    public Slider HealthBar;
    

    void Awake()
    {
        Globals.Player = gameObject;
        Gun = GetComponentInChildren<Gun>();
        Input = GetComponent<InputController>();
        AbilityController = GetComponent<AbilityController>();
        Movement = GetComponent<PlayerMovement>();
        Animation = GetComponent<PlayerAnimation>();        
        Health = GetComponent<Health>();
        HealthBar.maxValue = Health.CurrentHealth;
    }

    public void Shoot()
    {
        
        if (ShootingEnabled && !Movement.Casting)
        {
            Gun.Shoot();
        }
    }

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

    private void Die()
    {
        Debug.Log("DIE BITCH");
        //TODO: Implement dieing
    }



}
