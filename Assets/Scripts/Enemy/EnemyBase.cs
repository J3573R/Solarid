using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class EnemyBase : MonoBehaviour
{

    public GameObject DeathEffect;    
    public Slider HealthBar;

    [SerializeField] private Vector3 _healthBarOffset;

    public enum State
    {
        Idle,
        Alert,
        Move,
        Attack
    }

    [SerializeField] protected float Damage = 5;
    protected EnemyBase.State CurrentState;
    protected EnemyStateBase CurrentStateObject;
    protected Health Health;

    protected virtual void Awake()
    {
        Health = GetComponent<Health>();
        HealthBar.maxValue = Health.CurrentHealth;
    }

    protected virtual void Update()
    {
        HealthBar.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + _healthBarOffset);
    }

    public void SetState(EnemyBase.State state)
    {
        if (CurrentStateObject != null)
        {
            Destroy(CurrentStateObject);
        }

        switch (state)
        {            
            case State.Idle:
                CurrentStateObject = gameObject.AddComponent<ChargerIdle>();
                CurrentState = state;
                break;
            case State.Alert:
                CurrentStateObject = gameObject.AddComponent<ChargerAlert>();
                CurrentState = state;
                break;
            case State.Move:
                CurrentStateObject = gameObject.AddComponent<ChargerMove>();
                CurrentState = state;
                break;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerBullet")
        {
            if (!TakeDamage(Globals.PlayerDamage) && CurrentState != State.Alert && CurrentState != State.Move && CurrentState != State.Attack)
            {
                SetState(EnemyBase.State.Alert);
            }            

            Destroy(other.gameObject);
        }
    }

    protected virtual void Die()
    {
        if (DeathEffect != null)
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            DeathEffect.GetComponent<ParticleSystem>().Play();
        }
        HealthBar.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public virtual bool TakeDamage(int damage)
    {
        if (Health.TakeDamage(damage))
        {
            Die();
            HealthBar.value = Health.CurrentHealth;
            return true;
        } else
        {
            HealthBar.value = Health.CurrentHealth;
            return false;
        }
        
    }


}
