using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttack : EnemyStateBase
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        base.Update();
        Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Attack);
    }
}
