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
    public float RecoilBuildUp;
    public float MaxRecoil;
    public bool Shooting;

    private float ShootingTime;
    private float _recoil;
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

    public void SetShooting(bool state)
    {
        if (state)
        {
            Shooting = state;
            ShootingTime = 0;
        }
        else
        {
            Shooting = false;
            _recoil = 0;
        }
    }

    void Update()
    {
        //Debug.Log(transform.position.y);
        if (_reload > 0)
        {
            _reload -= Time.deltaTime * 1;
        }

        if (Shooting)
        {
            ShootingTime += Time.deltaTime;
           if (_recoil < MaxRecoil)
            {
                _recoil += RecoilBuildUp;
            }
        }
        //Debug.Log(_recoil);
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
                    _bullets[i].transform.position = new Vector3(_collider.transform.position.x, 1.5f, _collider.transform.position.z);                    
                    _target.y = 1.5f;
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
        Vector3 direction = _player.transform.forward;
        //Debug.Log(direction);

        float tmp = UnityEngine.Random.Range(-_recoil, _recoil);
        //Debug.Log(tmp);
        Vector3 vec = Quaternion.AngleAxis(tmp, Vector3.forward) * direction;

        return transform.position + direction * TargetDistance;
    }
}
