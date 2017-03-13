using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour {

    public float CoolDown;
    public float CoolDownRemaining;
    public float MaxRange;

    public abstract void Execute();

    /// <summary>
    /// Returns remaining cooldown
    /// </summary>
    /// <returns>Obvious isn't it...?</returns>
    public float GetRemainingCooldown()
    {
        return CoolDownRemaining;
    }

    public virtual void Update()
    {
        if (CoolDownRemaining >= 0)
        {
            CoolDownRemaining -= Time.deltaTime;
        }
    }

    public float GetRange()
    {
        return MaxRange;
    }

    
}
