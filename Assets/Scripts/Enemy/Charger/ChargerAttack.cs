﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAttack : EnemyStateBase
{
    // Time between damage ticks to player
    
    private float _distance;
    private Charger _charger;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Idle;
        _charger = (Charger)Parent;
    }

    protected override void Update()
    {
        _distance = Vector3.Distance(transform.position, Parent.Target.transform.position);

        // If distance is bigger than 2 change back to move, else attack
        if (_distance > 2)
        {
            Parent.SetState(EnemyBase.State.Move);
        }
        else
        {
            if (_charger.AttackTimer >= _charger.TimeBetweenAttacks)
            {
                Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Attack);
                Parent.Target.GetComponent<Health>().TakeDamage(Parent.Damage);
                _charger.AttackTimer = 0;
            }
        }
    }
}
