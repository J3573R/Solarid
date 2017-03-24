using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shielder : EnemyBase
{

    // Changes enemy state to alert when distance is smaller than this
    public int AlertDistance = 5;

    // Changes enemy state to idle when distance is bigger than this
    public int DisengageDistance = 7;

    // Attack speed of the enemy
    public float TimeBetweenAttacks = 1f;

    // Time after last attack
    public float AttackTimer = 0;

    public GameObject Shield;

    private Vector3 _shieldOffset;

    protected override void Awake()
    {
        base.Awake();
        SetState(EnemyBase.State.Idle);
        StartPosition = transform.position;
        _shieldOffset = Shield.transform.position - transform.position;
    }

    protected override void Update()
    {
        base.Update();
        Shield.transform.position = transform.position + transform.forward; //+ _shieldOffset;
        Shield.transform.rotation = transform.rotation;

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
                    CurrentStateObject = gameObject.AddComponent<ShielderIdle>();
                    CurrentState = state;
                    break;
                case State.Alert:
                    CurrentStateObject = gameObject.AddComponent<ShielderAlert>();
                    CurrentState = state;
                    AlertOthers();
                    break;
                case State.Move:
                    CurrentStateObject = gameObject.AddComponent<ShielderMove>();
                    CurrentState = state;
                    break;
                case State.Attack:
                    CurrentStateObject = gameObject.AddComponent<ShielderAttack>();
                    CurrentState = state;
                    break;
            }
        }
    }
}
