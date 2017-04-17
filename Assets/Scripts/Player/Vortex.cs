using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Vortex : MonoBehaviour
{

    public int DamagePerSecond = 50;
    public float Lifetime = 5f;

    private float _damageTickTime = 0;

	void Update () {

        if (!GameStateManager.Instance.GameLoop.Paused)
        {
            if (_damageTickTime <= 0)
            {
                _damageTickTime = 1;
            }

            _damageTickTime -= Time.deltaTime;
            Collider[] enemyColliders = Physics.OverlapSphere(transform.position, 5);
            foreach (Collider enemy in enemyColliders)
            {
                EnemyBase tmp = enemy.gameObject.GetComponent<EnemyBase>();
                if (tmp != null)
                {
                    tmp.PullToPoint(transform.position, 0.1f);

                    if (_damageTickTime <= 0)
                    {
                        tmp.TakeDamage(DamagePerSecond);
                    }
                }
            }

            if (Lifetime <= 0)
            {
                Destroy(gameObject);
            }
            Lifetime -= Time.deltaTime;
        }        
    }
}
