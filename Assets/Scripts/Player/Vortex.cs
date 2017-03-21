using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Vortex : MonoBehaviour {

    public float Lifetime = 5f;
    
	void Update () {
        if(Lifetime <= 0)
        {
            Destroy(gameObject);
        }

        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider enemy in enemyColliders)
        {
            EnemyBase tmp = enemy.gameObject.GetComponent<EnemyBase>();
            if (tmp != null)
            {
                tmp.PullToPoint(transform.position, 0.1f);
                tmp.AlertOthers();
            }
        }

        Lifetime -= Time.deltaTime;
    }
}
