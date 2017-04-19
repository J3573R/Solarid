using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class EnemyBase : MonoBehaviour
{
    public int Damage;
    public GameObject DeathEffect;
    public GameObject HealthBar;
    public Health Health;
    public Vector3 StartPosition;
    public Animator Animator;
    public float RangeToAlert = 1;
    public float ChaseTime = 3;
    public GameObject Target;
    public GameObject AttackTarget;
    [HideInInspector] public bool Freeze = false;

    // Changes enemy state to alert when distance is smaller than this
    public int AlertDistance = 5;

    // Changes enemy state to idle when distance is bigger than this
    public int DisengageDistance = 7;

    public bool Initialized = false;

    public GameObject[] Weapons;

    protected NavMeshAgent Agent;

    private Slider _healthBar;
    private bool _showHealth = false;
    private Player _player;

    // Pull effect for black hole
    private Vector3 _pullPoint;
    private float _pullDuration;
    private bool _pullActive = false;

    
    
    public enum State
    {
        None,
        Idle,
        Alert,
        Move,
        Attack
    }

    public enum AnimationState
    {
        Idle = 0,
        Walk = 1,
        Attack = 2,
        WalkBack = 3,
        Death = 4        
    }

    protected EnemyBase.State CurrentState;
    protected EnemyStateBase CurrentStateObject;

    [SerializeField]
    private Vector3 _healthBarOffset = Vector3.zero;

    protected virtual void Start()
    {
        if (!Initialized)
        {
            return;
        }
    }

    protected virtual void Init()
    {
        Health = GetComponent<Health>();
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        if (GameStateManager.Instance.GameLoop.Player.gameObject != null)
            _player = GameStateManager.Instance.GameLoop.Player.gameObject.GetComponent<Player>();
        GameObject bar = Instantiate(HealthBar);
        bar.transform.SetParent(GameObject.Find("UI").transform);
        _healthBar = bar.GetComponent<Slider>();
        _healthBar.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        _healthBar.maxValue = Health.CurrentHealth;
        _healthBar.gameObject.SetActive(false);
        Target = GetClosestTarget();
    }

    protected virtual void Update()
    {
        if (!Initialized)
        {
            if (GameStateManager.Instance.GameLoop.PlayerReady)
            {
                Init();
            }
            return;
        }

        if (GameStateManager.Instance.GameLoop.Paused && !Freeze)
        {
            CurrentStateObject.enabled = false;
            _healthBar.gameObject.SetActive(false);
            Agent.enabled = false;
            Animator.speed = 0;
            Freeze = true;
        } else if (!GameStateManager.Instance.GameLoop.Paused && Freeze && !Health.IsDead())
        {
            CurrentStateObject.enabled = true;
            _healthBar.gameObject.SetActive(true);
            Agent.enabled = true;
            Animator.speed = 1;
            Freeze = false;
        }

        if (Freeze)
        {
            return;
        }

        Target = GetClosestTarget();
        _healthBar.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + _healthBarOffset);

        if(!_showHealth && Health.CurrentHealth < _healthBar.maxValue)
        {
            _healthBar.gameObject.SetActive(true);
            _showHealth = true;
        }

        if(_pullActive && _pullDuration > 0)
        {
            var direction = _pullPoint - transform.position;
            transform.position += direction * Time.deltaTime;
            _pullDuration -= Time.deltaTime;
        }

        if (_pullActive && _pullDuration <= 0)
        {
            _pullActive = false;
            Agent.enabled = true;
        }
    }

    /// <summary>
    /// Creates and changes state for enemy.
    /// </summary>
    /// <param name="state">State to change</param>
    public virtual void SetState(EnemyBase.State state)
    {
        Debug.LogWarning("SetState not implemented.");
    }

    /// <summary>
    /// Reduces the damage from enemys health.
    /// </summary>
    /// <param name="damage">Amount of damage caused to enemy.</param>
    /// <returns>If dead true, otherwise false</returns>
    public virtual bool TakeDamage(int damage)
    {
        if (Health.TakeDamage(damage))
        {
            Die();
            _healthBar.value = Health.CurrentHealth;
            return true;
        } else
        {
            AlertOthers();
            SetState(EnemyBase.State.Move);
            _healthBar.value = Health.CurrentHealth;
            return false;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerBullet")
        {
            TakeDamage(_player.Damage);            
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "PlayerBullet")
        {
            TakeDamage(_player.Damage);
        }
    }

    /// <summary>
    /// Kills enemy.
    /// </summary>
    protected virtual void Die()
    {
        if (DeathEffect != null)
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            DeathEffect.GetComponent<ParticleSystem>().Play();
        }

        GameStateManager.Instance.GameLoop.References.ManaExplosion.Explode(transform.position);

        _healthBar.gameObject.SetActive(false);
        //CurrentStateObject.gameObject.SetActive(false);        

        Collider[] colliders = GetComponentsInChildren<Collider>();
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

        Agent.enabled = false;
        Animator.enabled = false;
        
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        Collider myCollider = GetComponent<Collider>();
        myCollider.enabled = false;

        foreach (var body in bodies)
        {
            body.useGravity = true;
            body.isKinematic = false;
            body.AddForce(-transform.forward / 20, ForceMode.Impulse);
        }
        
        if(Weapons != null)
        {
            foreach(GameObject weapon in Weapons)
            {
                weapon.transform.parent = null;
            }
        }

        Freeze = true;
        Destroy(CurrentStateObject);
        CurrentState = State.None;
    }

    /// <summary>
    /// Alerts other enemies from range around them.
    /// </summary>
    public virtual void AlertOthers()
    {
        if(CurrentState != EnemyBase.State.Alert)
        {
            SetState(State.Alert);
        }
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            try
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) <= RangeToAlert)
                {
                    enemy.GetComponent<EnemyBase>().SetState(State.Alert);
                }
            }
            catch (Exception e)
            {
                Debug.Log("ERROR CATCHED:");
                Debug.LogError(e.Message);
            }
        }
    }

    /// <summary>
    /// Pulls enemy towards point.
    /// </summary>
    /// <param name="point">Point to pull towards.</param>
    /// <param name="duration">Duration of the pull.</param>
    public virtual void PullToPoint(Vector3 point, float duration)
    {
        _pullPoint = point;
        _pullDuration = duration;
        _pullActive = true;
        Agent.enabled = false;
    }

    /// <summary>
    /// Checks if navigation agent is moving.
    /// </summary>
    /// <returns>False if nav mesh is reached target, otherwise true</returns>
    public bool IsNavMeshMoving()
    {
        if (Agent.isActiveAndEnabled && !Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public GameObject GetClosestTarget()
    {
        GameObject target = GameStateManager.Instance.GameLoop.Player.gameObject.gameObject;
        float targetDistance = Vector3.Distance(transform.position, target.transform.position);

        foreach (var clone in _player.Clones)
        {
            if (clone != null && clone.activeInHierarchy)
            {
                float distance = Vector3.Distance(transform.position, clone.transform.position);
                if (distance < targetDistance)
                {
                    target = clone;
                    targetDistance = distance;
                }
            }
        }

        return target;
    }

    public void InflictDirectDamage()
    {
        if (Target.Equals(AttackTarget))
        {
            Target.GetComponent<Health>().TakeDamage(Damage);
        }
    }

}
