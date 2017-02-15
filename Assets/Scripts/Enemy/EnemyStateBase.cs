using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateBase : MonoBehaviour
{
    public NavMeshAgent Agent;
    public EnemyBase.State eState;

    protected EnemyBase Parent;

    public EnemyBase.State State
    {
        get { return eState; }
    }

    protected virtual void Awake()
    {
        Parent = GetComponent<EnemyBase>();
        Agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update(){}
    protected virtual void OnEnable(){}
    protected virtual void OnDisable(){}
}
