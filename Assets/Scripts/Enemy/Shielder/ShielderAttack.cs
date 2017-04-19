using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderAttack : EnemyStateBase
{
    private float _distance;

    protected override void Init()
    {
        base.Init();
        Parent.AttackTarget = Parent.Target;
        eState = EnemyBase.State.Idle;
    }

    protected override void Update()
    {
        base.Update();
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
