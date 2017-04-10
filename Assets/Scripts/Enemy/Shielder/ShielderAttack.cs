using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderAttack : EnemyStateBase
{
    // Time between damage ticks to player
    
    private float _distance;
    private Shielder _charger;

    protected override void Start()
    {
        base.Start();
        Parent.AttackTarget = Parent.Target;
        eState = EnemyBase.State.Idle;
        _charger = (Shielder)Parent;
    }

    protected override void Update()
    {
        _distance = Vector3.Distance(transform.position, Parent.Target.transform.position);

        // If distance is bigger than 2 change back to move, else attack
        if (_distance > 3)
        {
            Parent.SetState(EnemyBase.State.Move);
        }
        else
        {
            Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Attack);
        }
    }
}
