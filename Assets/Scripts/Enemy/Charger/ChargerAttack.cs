using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAttack : EnemyStateBase
{
    // Time between damage ticks to player
    
    private float _distance;
    private Charger _charger;
    private float _rotationSpeed = 5;
    private float _step;
    private Vector3 _targetDirection;
    private Vector3 _newDirection;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Idle;
        _charger = (Charger)Parent;
    }

    protected override void Update()
    {
        LookTarget();
        _distance = Vector3.Distance(transform.position, Parent.Target.transform.position);

        // If distance is bigger than 2 change back to move, else attack
        if (_distance > 2)
        {
            Parent.SetState(EnemyBase.State.Move);
        }
        else
        {
            if (_charger.AttackTimer >= _charger.TimeBetweenAttacks)
            {
                Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Attack);
                Parent.Target.GetComponent<Health>().TakeDamage(Parent.Damage);
                _charger.AttackTimer = 0;
            }
        }
    }

    /// <summary>
    /// Looks at target.
    /// </summary>
    private void LookTarget()
    {
        _targetDirection = Parent.Target.transform.position - transform.position;
        _step = _rotationSpeed * Time.deltaTime;
        _newDirection = Vector3.RotateTowards(transform.forward, _targetDirection, _step, 0.0F);
        transform.rotation = Quaternion.LookRotation(_newDirection);
        _targetDirection = Parent.Target.transform.position - transform.position;
    }
}
