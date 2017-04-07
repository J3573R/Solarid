using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaGlobe : MonoBehaviour {

    public float MaxSpeed;

    private Vector3 _moveDirection;
    private float _speed = -2;

    void Awake()
    {
        _moveDirection = new Vector3(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100)).normalized;
    }
	
	void FixedUpdate () {
        if(_speed < 0)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + -_moveDirection * _speed, Time.deltaTime);
        } else
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + _moveDirection * _speed, Time.deltaTime);
        }        

        if(_speed > 0)
        {
            _moveDirection = Globals.Player.transform.position - transform.position;
            _moveDirection = _moveDirection.normalized;
        }

        if(_speed < MaxSpeed)
        {
            _speed += Time.deltaTime;
        }
	}

    // TODO: Return me correctly to pool
    void ReturnToPool()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ReturnToPool();
        }
    }

}
