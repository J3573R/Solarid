using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charger : EnemyBase
{
    // Attack speed of the enemy
    public float TimeBetweenAttacks = 0.2f;

    // Time after last attack
    public float AttackTimer = 0;

    public ChargerAnimationTracker animTracker;

    private Vector3 _positionAtLastFrame;

    protected override void Start()
    {
        base.Start();
        if (CurrentState == State.None)
        {
            SetState(EnemyBase.State.Idle);
        }
        StartPosition = transform.position;

        animTracker = GetComponentInChildren<ChargerAnimationTracker>();
    }

    protected override void Update()
    {
        base.Update();

        if (Animator != null && CurrentState != State.Attack)
        {
            if (_positionAtLastFrame == transform.position)
            {
                Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Idle);
            }
            else
            {
                Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Walk);
            }

            _positionAtLastFrame = transform.position;
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
