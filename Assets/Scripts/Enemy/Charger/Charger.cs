using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charger : EnemyBase
{
    // Attack speed of the enemy
    public float TimeBetweenAttacks = 1f;

    // Time after last attack
    public float AttackTimer = 0;

    protected override void Start()
    {
        base.Start();
        if (CurrentState == State.None)
        {
            SetState(EnemyBase.State.Idle);
        }
        StartPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        if(AttackTimer < TimeBetweenAttacks)
        {
            AttackTimer += Time.deltaTime;
        }        
    }

    /// <summary>
    /// Creates and changes state for enemy.
    /// </summary>
    /// <param name="state">State to change</param>
    public override void SetState(EnemyBase.State state)
    {        
        if (CurrentState != state)
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
                    AlertOthers();
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
}
