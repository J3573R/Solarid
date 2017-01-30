using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Gun _gun;

    void Awake()
    {
        _gun = GetComponentInChildren<Gun>();
    }

    public void Shoot()
    {
        _gun.Shoot();
    }

}
