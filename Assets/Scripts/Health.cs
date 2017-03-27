using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] protected int _health;

    public int CurrentHealth
    {
        get { return _health; }
        private set { _health = (int)Mathf.Clamp(value, 0, Mathf.Infinity); }
    }
    
    virtual public bool TakeDamage(int damage)
    {
        if (!IsDead())
        {
            CurrentHealth -= damage;
        }

        return IsDead();        
    }

    public bool IsDead()
    {
        if(CurrentHealth <= 0)
        {
            return true;
        }

        return false;
    }
}
