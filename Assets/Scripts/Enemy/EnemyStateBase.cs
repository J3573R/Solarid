using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateBase : MonoBehaviour
{
    public NavMeshAgent Agent;
    public EnemyBase.State eState;

    protected EnemyBase Parent;
    protected bool Initialized = false;

    private float _distance;
    private float _transitionToAlert;

    public EnemyBase.State State
    {
        get { return eState; }
    }

    void Awake()
    {
        Parent = GetComponent<EnemyBase>();
    }

    protected virtual void Init()
    {
        Agent = GetComponent<NavMeshAgent>();
        _transitionToAlert = 0;
        Initialized = true;
    }

    protected virtual void Update()
    {
        if (Parent == null || !Parent.Initialized)
        {
            return;
        }

        if (!Initialized)
        {
            Init();
        }
    }
    protected virtual void OnEnable(){}
    protected virtual void OnDisable(){}

    /// <summary>
    /// Changes enemys state to alert if distance is small enough.
    /// </summary>
    protected void ChangeToAlert()
    {
        if(Parent.Target != null)
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
}
