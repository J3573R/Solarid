﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAlert : EnemyStateBase {

    private float _timer = 0.5f;
    private float _rotationSpeed = 2;
    private float _step;
    private Vector3 _targetDirection;
    private Vector3 _newDirection;

    protected override void Init()
    {
        base.Init();
        eState = EnemyBase.State.Alert;
    }

    protected override void Update()
    {
        base.Update();
        LookTarget();
        ChangeToMove();
    }

    /// <summary>
    /// Looks at target.
    /// </summary>
    private void LookTarget()
    {
        _targetDirection = Parent.Target.transform.position - transform.position;
        _step = _rotationSpeed * Time.deltaTime;
        _newDirection = Vector3.RotateTowards(transform.forward, _targetDirection, _step, 0.0F);
        transform.rotation = Quaternion.LookRotation(_newDirection);
        _targetDirection = Parent.Target.transform.position - transform.position;
    }

    /// <summary>
    /// Changes state to move after countdown is 0.
    /// </summary>
    private void ChangeToMove()
    {
        if (_timer <= 0)
        {
            Parent.SetState(EnemyBase.State.Move);
        }

        _timer -= Time.deltaTime;
    }
}
