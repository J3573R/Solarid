using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAttack : EnemyStateBase
{
    // Time between damage ticks to player
    public float TimeBetweenAttacks = 2;

    private float _attackTimer;
    private float _distance;

    bool attack = true;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Idle;
        _attackTimer = TimeBetweenAttacks;
    }

    protected override void Update()
    {
        _distance = Vector3.Distance(transform.position, Globals.Player.transform.position);

        // If distance is bigger than 2 change back to move, else attack
        if (_distance > 2)
        {
            Parent.SetState(EnemyBase.State.Move);
        }
        else
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= TimeBetweenAttacks)
            {
                //Debug.Log("ATTACK");
                if (attack)
                {

                    Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Attack);
                    attack = false;
                }

                Globals.Player.GetComponent<Player>().TakeDamage(Parent.Damage);
                _attackTimer = 0;
            }
        }
    }
}
