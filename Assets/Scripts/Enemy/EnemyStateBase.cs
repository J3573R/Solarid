using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateBase : MonoBehaviour
{
    public NavMeshAgent Agent;
    public EnemyBase.State eState;

    protected EnemyBase Parent;

    private float _distance;
    private float _transitionToAlert;

    public EnemyBase.State State
    {
        get { return eState; }
    }

    protected virtual void Awake()
    {
        Parent = GetComponent<EnemyBase>();
        Agent = GetComponent<NavMeshAgent>();
        _transitionToAlert = 0;
    }

    protected virtual void Update(){}
    protected virtual void OnEnable(){}
    protected virtual void OnDisable(){}

    /// <summary>
    /// Changes enemys state to alert if distance is small enough.
    /// </summary>
    protected void ChangeToAlert()
    {
        _distance = Vector3.Distance(transform.position, Parent.Target.transform.position);

        if (_distance < Parent.AlertDistance)
        {
            _transitionToAlert += Time.deltaTime;
            if (_transitionToAlert >= 0.5f)
            {
                Parent.SetState(EnemyBase.State.Alert);
            }
        }
        else
        {
            _transitionToAlert = 0;
        }
    }
}
