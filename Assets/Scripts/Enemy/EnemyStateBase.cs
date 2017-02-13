using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBase
{

    protected EnemyBase.State eState;
    protected GameObject Parent;

    public EnemyStateBase(GameObject parent)
    {
        Parent = parent;
    }

    public EnemyBase.State State
    {
        get { return eState; }
    }

    public virtual void Update()
    {
        
    }

    protected virtual void Activate()
    {
        
    }

    protected virtual void Disable()
    {
        
    }
}
