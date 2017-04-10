﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaGlobe : MonoBehaviour {

    public float MaxSpeed;
    public float Speed;

    private Vector3 _moveDirection;
    private float _speed = -5;
    private Vector3 _moveToPoint;

    void OnEnable()
    {
        _speed = -5;
        _moveDirection = new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100)).normalized;
    }
	
	void FixedUpdate () {

        if(_speed < 0)
        {
            _moveToPoint = transform.position + -_moveDirection * _speed;
        } else
        {
            _moveToPoint = transform.position + _moveDirection * _speed;
            _moveToPoint.y = 0.5f;            
        }

        transform.position = Vector3.Lerp(transform.position, _moveToPoint, Time.deltaTime);

        if (_speed > 0)
        {
            _moveDirection = GameStateManager.Instance.GameLoop.Player.gameObject.transform.position - transform.position;
            _moveDirection = _moveDirection.normalized;            
        }

        if(_speed < MaxSpeed)
        {
            _speed += Time.deltaTime * Speed;
        }
	}
    
    void ReturnToPool()
    {
        GameStateManager.Instance.GameLoop.References.ManaExplosion.AddToPool(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ReturnToPool();
        }
    }

}
