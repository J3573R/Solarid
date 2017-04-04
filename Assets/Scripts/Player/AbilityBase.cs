using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour {

    // Max range, set in editor
    public float MaxRange;
    // Mana cost of the ability
    public int ManaCost;

    /// <summary>
    /// Tries to execute the ability
    /// </summary>
    public abstract void Execute(Vector3 targetPos);

    /// <summary>
    /// Returns Max range of the skill
    /// </summary>
    /// <returns>Max range of the skill, zero if no range/selfcast</returns>
    public float GetRange()
    {
        return MaxRange;
    }    
}
