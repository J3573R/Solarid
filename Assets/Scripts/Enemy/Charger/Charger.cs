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
    public AudioClip AudioWalk;

    public ChargerAnimationTracker animTracker;

    private Vector3 _positionAtLastFrame;
    private AudioSource _audio;

    protected override void Init()
    {
        base.Init();
        if (CurrentState == State.None)
        {
            SetState(EnemyBase.State.Idle);
        }
        StartPosition = transform.position;
        _audio = GetComponent<AudioSource>();
        animTracker = GetComponentInChildren<ChargerAnimationTracker>();
        Initialized = true;
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
                if (!_audio.isPlaying)
                {
                    _audio.clip = AudioWalk;
                    _audio.loop = false;
                    _audio.Play();
                }
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
        if (CurrentState != state && !Dead)
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
