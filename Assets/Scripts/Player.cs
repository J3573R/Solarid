using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool ShootingEnabled = true;


    private Gun _gun;

    void Awake()
    {
        _gun = GetComponentInChildren<Gun>();
    }

    public void Shoot()
    {
        if (ShootingEnabled)
        {
            _gun.Shoot();
        }
    }
    
}
