using System;
using UnityEngine;
using UnityEngine.AI;

public class ShielderMove : EnemyStateBase
{
    private Shielder _parent;
    private float _distance;
    private float _followTime;
    private float _rotationSpeed = 2;
    private float _step;
    private Vector3 _targetDirection;
    private Vector3 _newDirection;

    protected override void Init()
    {
        base.Init();
        eState = EnemyBase.State.Move;
        Agent.speed = 2f;

        try
        {
            _parent = (Shielder)Parent;
        }
        catch (Exception e)
        {
            Debug.LogError("Parent was not Shielder in ShielderMove: " + e.Message);
        }
    }

    protected override void Update()
    {
        base.Update();
        _followTime += Time.deltaTime;
        if(Agent.isActiveAndEnabled)
            Agent.destination = Parent.Target.transform.position;
        _targetDirection = Parent.Target.transform.position - transform.position;
        _step = _rotationSpeed * Time.deltaTime;
        _newDirection = Vector3.RotateTowards(transform.forward, _targetDirection, _step, 0.0F);
        transform.rotation = Quaternion.LookRotation(_newDirection);

        // Based on distance, ether attack the player or change to idle state
        _distance = Vector3.Distance(transform.position, Parent.Target.transform.position);

        if (_distance <= 3)
        {
            Parent.SetState(EnemyBase.State.Attack);
        }
        else if(_distance >= _parent.DisengageDistance && _followTime >= Parent.ChaseTime)
        {
            Parent.SetState(EnemyBase.State.Idle);
        }
    }
}