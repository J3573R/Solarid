using System;
using UnityEngine;
using UnityEngine.AI;

public class ChargerMove : EnemyStateBase
{
    private Charger _parent;
    private float _distance;
    private float _followTime;
    private float _rotationSpeed = 2;
    private float _step;
    private Vector3 _targetDirection;
    private Vector3 _newDirection;

    protected override void Start()
    {
        base.Start();
        eState = EnemyBase.State.Move;
        Agent.speed = 5;

        try
        {
            _parent = (Charger)Parent;
        }
        catch (Exception e)
        {
            Debug.LogError("Parent was not Charger in ChargerMove: " + e.Message);
        }
    }

    protected override void Update()
    {
        _followTime += Time.deltaTime;
        Agent.destination = Parent.Target.transform.position;
        _targetDirection = Parent.Target.transform.position - transform.position;
        _step = _rotationSpeed * Time.deltaTime;
        _newDirection = Vector3.RotateTowards(transform.forward, _targetDirection, _step, 0.0F);
        transform.rotation = Quaternion.LookRotation(_newDirection);   

        // Based on distance, ether attack the player or change to idle state
        _distance = Vector3.Distance(transform.position, Parent.Target.transform.position);

        if (_distance <= 2)
        {
            Parent.SetState(EnemyBase.State.Attack);
        }
        else if(_distance >= _parent.DisengageDistance && _followTime >= Parent.ChaseTime)
        {
            Parent.SetState(EnemyBase.State.Idle);
        }
    }
}