﻿using System;
using UnityEngine;
using UnityEngine.AI;

public class ChargerIdle : EnemyStateBase
{
    private float _timeToWalk;
    private bool Idling;

    protected override void Start()
    {
        base.Start();
        eState = EnemyBase.State.Idle;
        _timeToWalk = 0;
        Agent.speed = 3.5f;
    }

    protected override void Update()
    {
        base.Update();
        Patrol();
        ChangeToAlert();
        if(Parent.IsNavMeshMoving() && !Idling)
        {
            Idling = true;
        }
    }    

    /// <summary>
    /// Gets random position from range, 
    /// finds closest navmesh position from start position and orders navmesh agent to move there after random period of time.
    /// </summary>
    private void Patrol()
    {
        if (_timeToWalk <= 0)
        {
            Vector3 randomDirection = (UnityEngine.Random.insideUnitSphere * 3) + Vector3.one;
            randomDirection += Parent.StartPosition;

            NavMeshHit navHit;
            if (Agent.isActiveAndEnabled && NavMesh.SamplePosition(randomDirection, out navHit, 1.0f, NavMesh.AllAreas))
            {
                Agent.destination = navHit.position;
                _timeToWalk = UnityEngine.Random.Range(1, 3);
                Idling = false;
            }
        }
        else
        {
            _timeToWalk -= Time.deltaTime;
        }
    }
}
