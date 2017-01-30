using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Bullet prefab
    public GameObject Bullet;
    // Reload time of the gun in seconds
    public float Reload;

    // Guns collider location
    private Collider _collider;
    // Current status of reload
    private float _reload = 0;

    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (_reload > 0)
        {
            _reload -= Time.deltaTime * 1;
        }
    }

    public void Shoot()
    {
        if (_reload <= 0)
        {
            Instantiate(Bullet, _collider.transform.position, transform.rotation);
            _reload = Reload;
        }
    }
}
