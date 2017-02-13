﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    public bool ShootingEnabled = true;
    
    private Gun _gun;
    public InputController input;
    [HideInInspector]
    public AbilityController abilityController;

    void Awake()
    {
        Globals.Player = gameObject;
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
