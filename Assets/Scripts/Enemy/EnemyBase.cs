﻿using System.Collections;
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
        HealthBar.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position);
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
            if (Health.TakeDamage(Globals.PlayerDamage))
            {
                Die();
            } else if (CurrentState != State.Alert && CurrentState != State.Move && CurrentState != State.Attack)
            {
                SetState(EnemyBase.State.Alert);
            }

            HealthBar.value = Health.CurrentHealth;

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


}
