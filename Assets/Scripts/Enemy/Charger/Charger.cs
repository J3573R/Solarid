using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charger : EnemyBase
{

    // Changes enemy state to alert when distance is smaller than this
    public int AlertDistance = 5;

    // Changes enemy state to idle when distance is bigger than this
    public int DisengageDistance = 7;
    
    protected override void Awake()
    {
        base.Awake();
        SetState(EnemyBase.State.Idle);
        StartPosition = transform.position;
    }

    /// <summary>
    /// Creates and changes state for enemy.
    /// </summary>
    /// <param name="state">State to change</param>
    public override void SetState(EnemyBase.State state)
    {
        if (CurrentStateObject != null)
        {
            Destroy(CurrentStateObject);
        }

        switch (state)
        {
            case State.Idle:
                CurrentStateObject = gameObject.AddComponent<ChargerIdle>();
                CurrentState = state;
                break;
            case State.Alert:
                CurrentStateObject = gameObject.AddComponent<ChargerAlert>();
                CurrentState = state;
                break;
            case State.Move:
                CurrentStateObject = gameObject.AddComponent<ChargerMove>();
                CurrentState = state;
                break;
            case State.Attack:
                CurrentStateObject = gameObject.AddComponent<ChargerAttack>();
                CurrentState = state;
                break;
        }
    }
}
