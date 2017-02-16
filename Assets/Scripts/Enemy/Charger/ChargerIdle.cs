using System;
using UnityEngine;
using UnityEngine.AI;

public class ChargerIdle : EnemyStateBase
{

    private Charger _parent;
    private float _timeToWalk;
    private float _distance;
    private float _transitionToAlert;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Idle;
        _timeToWalk = 2;
        _transitionToAlert = 0;
        Agent.speed = 3.5f;

        try
        {
            _parent = (Charger) Parent;
        }
        catch (Exception e)
        {
            Debug.LogError("Parent was not Charger in ChargerIdle: " + e.Message);
        }
        
    }

    protected override void Update()
    {
        Patrol();
        ChangeToAlert();
    }

    /// <summary>
    /// Gets random position from range, 
    /// finds closest navmesh position from start position and orders navmesh agent to move there after random period of time.
    /// </summary>
    private void Patrol()
    {
        if (_timeToWalk <= 0)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 3;
            randomDirection += Parent.StartPosition;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection, out navHit, 1.0f, NavMesh.AllAreas))
            {
                Agent.destination = navHit.position;
                _timeToWalk = UnityEngine.Random.Range(1, 3);
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
