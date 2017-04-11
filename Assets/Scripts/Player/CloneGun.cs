using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloneGun : MonoBehaviour
{
    // Bullet prefab
    public GameObject Bullet;
    // BulletInterval time of the gun in seconds
    public float BulletInterval;
    // Amount of bullets in pool
    public int PooledBullets;
    // pool of bullets
    public List<GameObject> _bullets;
    // Target to shoot
    private Vector3 _target;
    // Guns collider location
    private Collider _collider;
    // Current status of reload
    private float _intervalTimer = 0;
    private AudioSource _audio;
    

    void Awake()
    {
        _collider = GetComponent<Collider>();
        SetupBulletPool();
        _audio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Sets up the object pool of bullets
    /// </summary>
    private void SetupBulletPool()
    {
        for (int i = 0; i < PooledBullets; i++)
        {
            GameObject tmpGO = Instantiate(Bullet, _collider.transform.position, Quaternion.identity) as GameObject;            
            tmpGO.SetActive(false);
            _bullets.Add(tmpGO);
        }
    }

    void Update()
    {
        if (_intervalTimer > 0)
        {
            _intervalTimer -= Time.deltaTime * 1;
        }
    }    

    /// <summary>
    /// Shoot function for the clone. Shoots the direction of the vector from transform.
    /// </summary>
    public void ShootDirection(Vector3 direction)
    {
        
        if (_intervalTimer <= 0 )
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (!_bullets[i].activeInHierarchy)
                {
                    _target = direction;
                    _bullets[i].transform.position = new Vector3(_collider.transform.position.x, 1.5f, _collider.transform.position.z);
                    _target.y = 1.5f;
                    _bullets[i].transform.LookAt(_target);
                    _bullets[i].SetActive(true);
                    _audio.Play();
                    break;
                }
            }
            _intervalTimer = BulletInterval;
        }
    }
}
