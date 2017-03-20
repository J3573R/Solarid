using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour {

    // Cooldown of the skill
    public float CoolDown;
    // Remaining cooldown
    public float CoolDownRemaining;
    // Max range, set in editor
    public float MaxRange;

    /// <summary>
    /// Tries to execute the ability
    /// </summary>
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

    /// <summary>
    /// Returns Max range of the skill
    /// </summary>
    /// <returns>Max range of the skill, zero if no range/selfcast</returns>
    public float GetRange()
    {
        return MaxRange;
    }    
}
