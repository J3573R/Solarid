using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool ShootingEnabled = true;
    
    private Gun _gun;
    public InputController input;
    [HideInInspector]
    public AbilityController abilityController;

    void Awake()
    {
        _gun = GetComponentInChildren<Gun>();
        input = GetComponent<InputController>();
        abilityController = GetComponent<AbilityController>();
    }

    public void Shoot()
    {
        if (ShootingEnabled)
        {
            _gun.Shoot();
        }
    }

    private void Update()
    {
        if (abilityController == null)
        {
            Debug.Log("NULL");
        }
    }
}
