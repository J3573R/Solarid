using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Speed of the bullet
    public float Speed = 5f;
    // Range of the bullet
    public float Range = 1f;

    // Maximium travelled distance
    private float _fMaxDist;
    // Lifetime of the bullet
    private float _time;

    public Player _player;
    private bool _active;

    void Awake()
    {
        _fMaxDist = Range / Speed;
        _player = FindObjectOfType<Player>();
        _time = 0;

    }

    private void OnDisable()
    {
        transform.position = _player.transform.position;
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
                gameObject.SetActive(false);
                _time = 0;
            }
            else
            {
                transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                _time += Time.deltaTime;
            }
        }
        
    }

}
