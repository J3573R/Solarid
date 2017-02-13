using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float Health = 50;
    [SerializeField] protected float Damage = 5;

    public enum State
    {
        Idle,
        Alert,
        Move,
        Attack,
        Die
    }
    
    protected EnemyStateBase CurrentState;

    protected virtual void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }

    /// <summary>
    /// Reduces health by amount of damage if alive.
    /// </summary>
    /// <param name="damage">Amount of damage to reduce from health</param>
    /// <returns>True if dead, false if not</returns>
    public bool TakeDamage(float damage)
    {
        if (!IsDead())
        {
            Health -= damage;
        }

        return IsDead();
    }

    /// <summary>
    /// Checks if dead.
    /// </summary>
    /// <returns>True if dead, false if not</returns>
    public bool IsDead()
    {
        if (Health <= 0)
        {
            return true;
        }

        return false;
    }
}
