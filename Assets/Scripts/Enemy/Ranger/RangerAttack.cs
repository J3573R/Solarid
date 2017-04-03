using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttack : EnemyStateBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Attack);
    }
}
