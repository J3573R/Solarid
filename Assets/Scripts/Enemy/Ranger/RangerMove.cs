using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangerMove : EnemyStateBase {

    private float _distance;
    private float _minDistance = 8;
    private float _maxDistance = 10;
    private float _chaseTime;
    private Vector3 _direction;
    private Vector3 _lookAtTarget;
    private Ranger _parent;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Move;
        _parent = (Ranger) Parent;
        Agent.updateRotation = false;
        _chaseTime = 0;
    }

    protected override void OnDisable()
    {
        Agent.updateRotation = true;
    }

    protected override void Update()
    {
        if(_chaseTime > 3)
        {
            Parent.SetState(EnemyBase.State.Idle);
        }

        _distance = Vector3.Distance(Globals.Player.transform.position, transform.position);

        if(_distance > _maxDistance)
        {
            Agent.stoppingDistance = _maxDistance;
            Agent.destination = Globals.Player.transform.position;
            _chaseTime += Time.deltaTime;
            
        } else if (_parent.ReadyToShoot <= 0)
        {
            Parent.SetState(EnemyBase.State.Attack);
            _chaseTime += Time.deltaTime;

        } else if(_distance < _minDistance)
        {            
            _direction = (Globals.Player.transform.position - transform.position).normalized;
            Agent.stoppingDistance = 0f;
            Agent.destination = transform.position + -_direction;
            _chaseTime += Time.deltaTime;
        }

        _lookAtTarget.Set(Globals.Player.transform.position.x, transform.position.y, Globals.Player.transform.position.z);
        transform.LookAt(_lookAtTarget);
    }


}
