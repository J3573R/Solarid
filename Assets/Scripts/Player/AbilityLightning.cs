using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLightning : AbilityBase
{
    [SerializeField]
    private GameObject _blast;
    private LightningBolt _bolt;

    // Use this for initialization
    void Start()
    {
        _blast = Instantiate(_blast, transform.position, Quaternion.identity);
        _blast.SetActive(false);
        _bolt = _blast.GetComponent<LightningBolt>();
    }

    /// <summary>
    /// Gets the mouseposition and throws the grenade to it
    /// </summary>
    public override void Execute(Vector3 targetPos)
    {
        float distance = Mathf.Infinity;
        EnemyBase closestEnemy = null;
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, 15);
        foreach (Collider enemy in enemyColliders)
        {
            EnemyBase tmp = enemy.gameObject.GetComponent<EnemyBase>();
            if (tmp != null)
            {
                var dist = Vector3.Distance(Globals.Player.transform.position, tmp.gameObject.transform.position);
                if (dist < distance)
                {
                    closestEnemy = tmp;
                }
            }
        }

        if (closestEnemy != null)
        {
            _blast.transform.position = transform.position;
            _bolt.TargetEnemy = closestEnemy;
            _blast.SetActive(true);
        }

        //TODO: Cooldown/mana stuff needed, for all abilities
    }
    
}
