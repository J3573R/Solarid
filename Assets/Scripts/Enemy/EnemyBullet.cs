﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Speed of the bullet
    public float Speed = 5f;
    // Range of the bullet
    public float Range = 1f;
    // Pool to return
    public RangerBulletPool MyPool;
    // Bullet damage
    public int Damage;

    // Maximium travelled distance
    private float _fMaxDist;
    // Lifetime of the bullet
    private float _time;
    
    private bool _active;

    void Awake()
    {
        _fMaxDist = Range / Speed;
        _time = 0;

    }

    private void OnEnable()
    {
        _active = true;
    }

    void Update()
    {
        if (_active)
        {
            if (_time > _fMaxDist)
            {
                MyPool.ReturnBullet(gameObject);
                _time = 0;
            }
            else
            {
                transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                _time += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (MyPool != null)
        {
            if (other.tag == "Player")
            {
                PlayerHealth hp = Globals.Player.GetComponent<PlayerHealth>();
                hp.TakeDamage(Damage);
                MyPool.ReturnBullet(gameObject);
            }

            if (other.tag.Equals("Prop"))
            {
                MyPool.ReturnBullet(gameObject);
            } else if (other.tag == "Clone")
            {
                MyPool.ReturnBullet(gameObject);
            }
        }
        else
        {
            Debug.Log("Bullet failed to return pool.");
        }
    }

}
