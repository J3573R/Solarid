using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBase : MonoBehaviour
{

    public EnemyBase.State eState;
    protected EnemyBase Parent;

    public EnemyBase.State State
    {
        get { return eState; }
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void Awake()
    {
        Parent = GetComponent<EnemyBase>();
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
        
    }
}
