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
        Agent = GetComponent<NavMeshAgent>();
        _timeToWalk = 2;
        _transitionToAlert = 0;

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
        if (_timeToWalk <= 0)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 5;
            randomDirection += transform.position;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection, out navHit, 1.0f, NavMesh.AllAreas))
            {
                Agent.destination = navHit.position;
                _timeToWalk = UnityEngine.Random.Range(2, 4);
            }
        }
        else
        {
            _timeToWalk -= Time.deltaTime;
        }

        _distance = Vector3.Distance(transform.position, Globals.Player.transform.position);        

        if(_distance < _parent.AlertDistance)
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
