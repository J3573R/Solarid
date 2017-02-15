using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAttack : EnemyStateBase
{

    private float _distance;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Idle;
    }

    protected override void Update()
    {
        _distance = Vector3.Distance(transform.position, Globals.Player.transform.position);
        if (_distance > 2)
        {
            Parent.SetState(EnemyBase.State.Move);
        }
    }
}
