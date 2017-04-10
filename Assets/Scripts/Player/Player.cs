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
    [HideInInspector] public PlayerHealth Health;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerAnimation Animation;
    [HideInInspector] public Gun Gun;
    [HideInInspector] public PlayerMana Mana;
    [HideInInspector] public List<GameObject> Clones;
    public bool ShootingEnabled = true;
    
    public bool Dead { get; set; }
    public bool Initialized;
    private bool _playerReady;

    void Awake()
    {
        init();
    }

    public void init()
    {
        if (!Initialized)
        {
            Globals.Player = gameObject;
            Gun = GetComponentInChildren<Gun>();
            Input = GetComponent<InputController>();
            AbilityController = GetComponent<AbilityController>();
            Movement = GetComponent<PlayerMovement>();
            Animation = GetComponent<PlayerAnimation>();
            Health = GetComponent<PlayerHealth>();
            Mana = GetComponent<PlayerMana>();
            Dead = false;
            Initialized = true;
        }
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

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ManaGlobe")
        {
            Mana.AddMana(10);
        }
    }

    private void Update()
    {
        if (!_playerReady)
        {
            if (Gun.Initialized && Input.Initialized && AbilityController.Initialized && Movement.Initialized
                && Animation.Initialized && Health.Initialized && Mana.Initialized)
            {
                Globals.GameLoop.PlayerReady = true;
                _playerReady = true;
            }
        }
    }
}
