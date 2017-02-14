using System;
using UnityEngine;
using UnityEngine.AI;

public class ChargerIdle : EnemyStateBase
{

    private Charger _parent;
    private float _timeToWalk;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Idle;
        Agent = GetComponent<NavMeshAgent>();
        _timeToWalk = 2;

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
            Agent.destination = randomDirection;

            _timeToWalk = UnityEngine.Random.Range(2, 4);
        }
        else
        {
            _timeToWalk -= Time.deltaTime;
        }

        RaycastHit hit;

        if (Physics.Linecast(transform.position, Globals.Player.transform.position, out hit))
        {
            if (hit.distance < _parent.AlertDistance)
            {
                Parent.SetState(EnemyBase.State.Alert);
            }
        }
    }
}
