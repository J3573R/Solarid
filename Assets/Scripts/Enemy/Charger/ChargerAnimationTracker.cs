using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAnimationTracker: MonoBehaviour {

    private EnemyBase Parent;

    void Awake()
    {
        Parent = GetComponentInParent<EnemyBase>();
    }

    void DamageTarget()
    {
        Parent.InflictDirectDamage();
    }
}
