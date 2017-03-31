using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] private int _health = 100;

    public int CurrentHealth
    {
        get { return _health; }
        set { _health = (int)Mathf.Clamp(value, 0, Mathf.Infinity); }
    }
    
    public bool TakeDamage(int damage)
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
