using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class EnemyBase : MonoBehaviour
{
    public int Damage;
    public GameObject DeathEffect;
    public GameObject HealthBar;
    public Vector3 StartPosition;
    public Animator Animator;
    public float RangeToAlert = 1;
    public float ChaseTime = 3;

    protected NavMeshAgent Agent;

    private Slider _healthBar;
    private bool _showHealth = false;
    private Vector3 _positionAtLastFrame;

    // Pull effect for black hole
    private Vector3 _pullPoint;
    private float _pullDuration;
    private bool _pullActive = false;

    // Confuse effect
    private Vector3 _confusionPosition;
    private float _confusionDuration;
    private float _timeToWalk;
    private bool _confusionActive = false;
    
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
        Death = 3
    }

    protected EnemyBase.State CurrentState;
    protected EnemyStateBase CurrentStateObject;
    protected Health Health;

    [SerializeField]
    private Vector3 _healthBarOffset = Vector3.zero;

    protected virtual void Awake()
    {
        Health = GetComponent<Health>();
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        GameObject bar = Instantiate(HealthBar);
        bar.transform.SetParent(GameObject.Find("UI").transform);
        _healthBar = bar.GetComponent<Slider>();
        _healthBar.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        _healthBar.maxValue = Health.CurrentHealth;
        _healthBar.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
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

        if (_confusionDuration > 0 && _confusionActive)
        {
            Confusion();
            _confusionDuration -= Time.deltaTime;
        }
        else if(_confusionActive)
        {
            CurrentStateObject.enabled = true;
            _confusionActive = false;
        }

        if(Animator != null && CurrentState != State.Attack)
        {
            if (_positionAtLastFrame == transform.position)
            {
                Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Idle);

            }
            else
            {
                Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Walk);
            }

            _positionAtLastFrame = transform.position;
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
            TakeDamage(Globals.PlayerDamage);            
        } else if (other.tag == "Confusion")
        {
            Confuse(5f);
        } else if (other.tag == "Lightning")
        {
            TakeDamage(Globals.PlayerDamage);
        }
    }

    /// <summary>
    /// Kills player.
    /// TODO: Death animation, level restart, disable controls.
    /// </summary>
    protected virtual void Die()
    {
        if (DeathEffect != null)
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            DeathEffect.GetComponent<ParticleSystem>().Play();
        }
        _healthBar.gameObject.SetActive(false);
        Destroy(gameObject);
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
    /// Confuses enemy, making it patrol for set amount of time.
    /// </summary>
    /// <param name="duration">Duration of confuse effect.</param>
    public virtual void Confuse(float duration)
    {
        _confusionPosition = transform.position;
        _confusionDuration = duration;
        _timeToWalk = 0;
        CurrentStateObject.enabled = false;
        _confusionActive = true;
    }

    /// <summary>
    /// Animation and patrol calculation for confusion.
    /// </summary>
    private void Confusion()
    {
        if (IsNavMeshMoving())
        {
            Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Walk);
        }
        else
        {
            Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Idle);
        }

        if (_timeToWalk <= 0)
        {
            Vector3 randomDirection = (UnityEngine.Random.insideUnitSphere * 3) + Vector3.one * 2;
            randomDirection += _confusionPosition;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection, out navHit, 1.0f, NavMesh.AllAreas))
            {
                Agent.destination = navHit.position;
                _timeToWalk = 1f;
            }
        }
        else
        {
            _timeToWalk -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Checks if navigation agent is moving.
    /// </summary>
    /// <returns>False if nav mesh is reached target, otherwise true</returns>
    public bool IsNavMeshMoving()
    {
        if (!Agent.pathPending)
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
}
