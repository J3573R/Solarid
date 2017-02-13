using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] private int _health = 100;

    public int CurrentHealth
    {
        get { return _health; }
    }
    
    public bool TakeDamage(int damage)
    {
        if (!IsDead())
        {
            _health -= damage;
        }

        return IsDead();        
    }

    public bool IsDead()
    {
        if(_health <= 0)
        {
            return true;
        }

        return false;
    }
}
