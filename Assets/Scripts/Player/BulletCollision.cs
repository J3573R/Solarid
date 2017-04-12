using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {

    private Bullet _bullet;

	// Use this for initialization
	void Start () {
        _bullet = GetComponentInParent<Bullet>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy") || other.tag.Equals("Prop"))
        {
            _bullet.BulletHit();
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag.Equals("Enemy") || other.collider.tag.Equals("Prop"))
        {
            _bullet.BulletHit();
        }

    }
}
