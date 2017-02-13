using UnityEngine;
using UnityEngine.AI;

public class ChargerMove : EnemyStateBase
{

    public NavMeshAgent Agent;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Move;
        Agent = GetComponent<NavMeshAgent>();
        
    }

    protected override void Update()
    {
        Agent.destination = Globals.Player.transform.position;

        RaycastHit hit;

        if (Physics.Linecast(transform.position, Globals.Player.transform.position, out hit))
        {
            if (hit.distance > 10)
            {
                Parent.SetState(EnemyBase.State.Idle);
            }
        }
    }
}