using System;
using UnityEngine;
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

    private Slider _healthBar;

    // Pull effect for black hole
    private Vector3 _pullPoint;
    private float _pullDuration;
    
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
        GameObject bar = Instantiate(HealthBar);
        bar.transform.SetParent(GameObject.Find("UI").transform);
        _healthBar = bar.GetComponent<Slider>();
        _healthBar.maxValue = Health.CurrentHealth;
    }

    protected virtual void Update()
    {
        _healthBar.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + _healthBarOffset);

        if (Input.GetKeyDown(KeyCode.F))
        {
            PullToPoint(Vector3.zero, 3f);
        }

        if(_pullDuration > 0)
        {
            var direction = _pullPoint - transform.position;
            transform.position += direction * Time.deltaTime * 2;
            _pullDuration -= Time.deltaTime;
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
            Bullet bullet = other.GetComponentInParent<Bullet>();
            bullet.BulletHit();
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

    public virtual void PullToPoint(Vector3 point, float duration)
    {
        _pullPoint = point;
        _pullDuration = duration;
    }

}
