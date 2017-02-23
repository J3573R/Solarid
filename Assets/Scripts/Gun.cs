using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Bullet prefab
    public GameObject Bullet;
    // Reload time of the gun in seconds
    public float Reload;
    // Amount of bullets in pool
    public int PooledBullets;
    // pool of bullets
    public List<GameObject> _bullets;

    public float TargetDistance;

    private Player _player;
    private Vector3 _target;
    private Vector3 _aimRotation;

    // Guns collider location
    private Collider _collider;
    // Current status of reload
    private float _reload = 0;

    void Awake()
    {
        _collider = GetComponent<Collider>();
        _player = FindObjectOfType<Player>();
        SetupBulletPool();
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
        if (_reload > 0)
        {
            _reload -= Time.deltaTime * 1;
        }
    }

    /// <summary>
    /// Searches object pool for inactive bullet and fires it
    /// </summary>
    public void Shoot()
    {
        if (_reload <= 0)
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (!_bullets[i].activeInHierarchy)
                {
                    _target = GetTargetPosition();
                    _bullets[i].transform.position = _collider.transform.position;
                    _target.y = _bullets[i].transform.position.y;
                    //Debug.Log(_target);
                    _bullets[i].transform.LookAt(_target);
                    _bullets[i].SetActive(true);
                    break;
                }
            }
            _reload = Reload;
        }
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 direction = _player.transform.TransformDirection(transform.forward);
        Debug.Log(direction);
        return direction * TargetDistance;
    }
}
