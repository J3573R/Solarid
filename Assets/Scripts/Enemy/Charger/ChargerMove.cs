using System;
using UnityEngine;
using UnityEngine.AI;

public class ChargerMove : EnemyStateBase
{
    private Charger _parent;
    private float _distance;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Move;

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
        // Follow player
        Agent.destination = Globals.Player.transform.position;

        // Based on distance, ether attack the player or change to idle state.
        _distance = Vector3.Distance(transform.position, Globals.Player.transform.position);

        if (_distance <= 2)
        {
            Parent.SetState(EnemyBase.State.Attack);
        }
        else if(_distance >= _parent.DisengageDistance)
        {
            Parent.SetState(EnemyBase.State.Idle);
        }
    }
}