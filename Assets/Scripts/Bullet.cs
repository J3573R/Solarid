using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Gun
{
    // Speed of the bullet
    public float Speed = 5f;
    // Range of the bullet
    public float Range = 1f;

    // Maximium travelled distance
    private float _fMaxDist;
    // Lifetime of the bullet
    private float _time;

    void Awake()
    {
        _fMaxDist = Range / Speed;
        _time = 0;
    }

    void Update()
    {
        if (_time > _fMaxDist)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector3.up * Speed * Time.deltaTime);
        _time += Time.deltaTime;
    }

}
