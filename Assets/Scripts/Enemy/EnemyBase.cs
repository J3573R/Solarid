using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class EnemyBase : MonoBehaviour
{
    public int Damage;
    public GameObject DeathEffect;
    public GameObject HealthBar;

    private Slider _healthBar;

    public enum State
    {
        Idle,
        Alert,
        Move,
        Attack
    }

    protected EnemyBase.State CurrentState;
    protected EnemyStateBase CurrentStateObject;
    protected Health Health;

    [SerializeField]
    private Vector3 _healthBarOffset = Vector3.zero;

    protected virtual void Awake()
    {
        Health = GetComponent<Health>();
        GameObject bar = Instantiate(HealthBar);
        bar.transform.parent = GameObject.Find("Canvas").transform;
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
            case State.Attack:
                CurrentStateObject = gameObject.AddComponent<ChargerAttack>();
                CurrentState = state;
                break;
        }
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
        }
        else
        {
            _healthBar.value = Health.CurrentHealth;
            return false;
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
