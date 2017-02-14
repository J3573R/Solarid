using System;
using UnityEngine;
using UnityEngine.AI;

public class ChargerMove : EnemyStateBase
{

    
    private Charger _parent;

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
        Agent.destination = Globals.Player.transform.position;

        RaycastHit hit;

        if (Physics.Linecast(transform.position, Globals.Player.transform.position, out hit))
        {
            if (hit.distance > _parent.DisengageDistance)
            {
                Parent.SetState(EnemyBase.State.Idle);
            }
        }
    }
}