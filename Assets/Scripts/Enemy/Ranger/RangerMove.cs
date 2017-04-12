﻿using System.Collections;
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

    protected override void Start()
    {
        base.Start();
        eState = EnemyBase.State.Move;
        _parent = (Ranger) Parent;
        Agent.updateRotation = false;
        _chaseTime = 0;
    }

    protected override void Update()
    {
        base.Update();
        if (IsNavMeshMoving())
        {
            Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Walk);
        }
        else if(!Parent.Animator.GetCurrentAnimatorStateInfo(0).IsName("Anubis_Idle"))
        {
            Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Idle);
        }

        _distance = Vector3.Distance(Parent.Target.transform.position, transform.position);

        if (_chaseTime > Parent.ChaseTime)
        {
            Agent.stoppingDistance = 0f;
            Parent.SetState(EnemyBase.State.Idle);
        }
        else if (_parent.ReadyToShoot <= 0)
        {
            Parent.SetState(EnemyBase.State.Attack);
            
        } else if (_distance > _maxDistance && Agent.isActiveAndEnabled)
        {
            Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Walk);
            Agent.stoppingDistance = _maxDistance;
            Agent.destination = Parent.Target.transform.position;
            
        }  else if(_distance < _minDistance && Agent.isActiveAndEnabled)
        {
            Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.WalkBack);
            _direction = (Parent.Target.transform.position - transform.position).normalized;
            Agent.stoppingDistance = 0f;
            Agent.destination = transform.position + -_direction;
        }

        _chaseTime += Time.deltaTime;
        _direction = (Parent.Target.transform.position - transform.position).normalized;
        _direction.y = 0;
        transform.rotation = Quaternion.LookRotation(_direction);
    }

    private bool IsNavMeshMoving()
    {
        if (!Agent.pathPending)
        {
            if (Agent.isActiveAndEnabled && Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    return false;
                }
            }
        }

        return true;
    }


}
