﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class EnemyBase : MonoBehaviour
{
    public int Damage;
    public GameObject DeathEffect;
    public GameObject HealthBar;
    public Vector3 StartPosition;
    public Animator Animator;

    private Slider _healthBar;
    
    public enum State
    {
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

            Destroy(other.gameObject);
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

    


}
