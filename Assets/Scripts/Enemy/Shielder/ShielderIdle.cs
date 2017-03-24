using System;
using UnityEngine;
using UnityEngine.AI;

public class ShielderIdle : EnemyStateBase
{

    private Shielder _parent;
    private float _timeToWalk;
    private float _distance;
    private float _transitionToAlert;
    private bool Idling;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Idle;
        _timeToWalk = 2;
        _transitionToAlert = 0;
        Agent.speed = 3.5f;

        try
        {
            _parent = (Shielder) Parent;
        }
        catch (Exception e)
        {
            Debug.LogError("Parent was not Shielder in ShielderIdle: " + e.Message);
        }
        
    }

    protected override void Update()
    {
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
            if (NavMesh.SamplePosition(randomDirection, out navHit, 1.0f, NavMesh.AllAreas))
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

    /// <summary>
    /// Changes enemys state to alert if distance is small enough.
    /// </summary>
    private void ChangeToAlert()
    {
        _distance = Vector3.Distance(transform.position, Globals.Player.transform.position);

        if (_distance < _parent.AlertDistance)
        {
            _transitionToAlert += Time.deltaTime;
            if (_transitionToAlert >= 0.5f)
            {
                Parent.SetState(EnemyBase.State.Alert);
            }
        }
        else
        {
            _transitionToAlert = 0;
        }
    }
}
