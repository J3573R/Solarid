using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GameObject Bullet;

    private Collider _collider;

    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(Bullet, _collider.transform.position, transform.rotation);
        
    }
}
