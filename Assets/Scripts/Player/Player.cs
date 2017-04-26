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
    public int Damage;
    public bool Interact = false;
    public bool Dead { get; set; }
    public bool Initialized;
    public SaveData.Crystal CrystalWithPlayer = SaveData.Crystal.none;
    public Dictionary<SaveData.Crystal, bool> HubCrystals;

    private bool _playerReady;

    void Awake()
    {
        init();
    }

    public void init()
    {
        if (!Initialized)
        {
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

    private void Update()
    {
        if (!_playerReady)
        {
            if (Gun.Initialized && Input.Initialized && AbilityController.Initialized && Movement.Initialized
                && Animation.Initialized && Health.Initialized && Mana.Initialized)
            {

                GameStateManager.Instance.GameLoop.PlayerReady = true;
                _playerReady = true;
            }
        }
    }

    public void Die()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

        //Animator.enabled = false;
        Animation.enabled = false;

        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        Collider myCollider = GetComponent<Collider>();
        myCollider.enabled = false;

        foreach (var body in bodies)
        {
            body.useGravity = true;
            body.isKinematic = false;
            body.AddForce(-transform.forward / 20, ForceMode.Impulse);
        }       
    }
}
