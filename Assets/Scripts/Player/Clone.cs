using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone : MonoBehaviour {

    public float Lifetime;
    public GameObject Hero;
    public GameObject HealthBar;
    public Vector3 HealthBarOffset;

    protected Health Health;

    private CloneGun _gun;
    private float _targetDistance = Mathf.Infinity;
    private GameObject _target;
    private EnemyBase _targetEnemyBase;
    private Animator _animator;
    private float _lifetime;
    private ParticleSystem _destroyEffect;
    private ParticleSystem _circleParticle;
    
    private bool _dying;
    private Slider _healthBar;
    private bool _showHealth = false;
    private bool _paused;

    void Awake()
    {
        Health = GetComponent<Health>();
        _gun = GetComponentInChildren<CloneGun>();
        _animator = GetComponentInChildren<Animator>();
        _destroyEffect = GetComponent<ParticleSystem>();
        GameObject bar = Instantiate(HealthBar);
        _circleParticle = transform.FindChild("CloneLight/CloneAura").GetComponent<ParticleSystem>();  
        bar.transform.SetParent(GameObject.Find("UI").transform);
        _healthBar = bar.GetComponent<Slider>();
        _healthBar.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        _healthBar.maxValue = Health.CurrentHealth;
        _healthBar.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Health.CurrentHealth = (int)_healthBar.maxValue;
        _showHealth = false;
        _lifetime = Lifetime;
        _destroyEffect.Stop();
        Hero.SetActive(true);
        _dying = false;
    }

    /// <summary>
    /// Destroy clone prematurely.
    /// </summary>
    public void Kill()
    {
        Die();
    }
		
	void Update () {

	    if (ListenPause())
	    {
	        return;
	    }

        _healthBar.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + HealthBarOffset);
	    _healthBar.value = Health.CurrentHealth;

        UpdateLifeTimeCircle();        

        // Activates health bar when damage is dealt.
        if (!_showHealth && Health.CurrentHealth < _healthBar.maxValue)
        {
            _healthBar.gameObject.SetActive(true);
            _showHealth = true;
        }

        if (!_dying)
        {
            // When target dies
            if (_targetEnemyBase != null && _targetEnemyBase.Freeze)
            {
                ResetTarget();
            }

            // Default behaviour
            if (_target == null)
            {
                _animator.SetInteger("animState", 0);
                SearchTarget();
            }
            else
            {
                _animator.SetInteger("animState", 1);
                Vector3 direction = _target.transform.position - transform.position;
                direction.y = 0;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = lookRotation;
                _gun.ShootDirection(_target.transform.position);
            }
            
            // If health depletes or time runs out
            if (_lifetime <= 0 || Health.CurrentHealth <= 0)
            {
                Die();
            }

            _lifetime -= Time.deltaTime;
        }
        else
        {
            if (!_destroyEffect.IsAlive())
            {
                gameObject.SetActive(false);
            }            
        }
    }

    private void UpdateLifeTimeCircle()
    {
        float timeLeftPercent = (_lifetime - 0.9f) / (Lifetime - 0.9f);

        if (timeLeftPercent < 0)
        {
            timeLeftPercent = 0;
            _circleParticle.Stop();
        }

        var tmp = _circleParticle.shape;
        tmp.arc = 360 * (timeLeftPercent);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet")
        {
            EnemyBullet bullet = other.gameObject.GetComponent<EnemyBullet>();
            Health.TakeDamage(bullet.Damage);
        }
    }

    /// <summary>
    /// Resets clone target.
    /// </summary>
    void ResetTarget()
    {
        _target = null;
        _targetEnemyBase = null;
        _targetDistance = 0;
    }

    /// <summary>
    /// Sets clone target.
    /// </summary>
    /// <param name="target">Gameobject with enemy tag and EnemyBase component.</param>
    void SetTarget(GameObject target)
    {
        _target = target;
        _targetEnemyBase = target.GetComponent<EnemyBase>();
        _targetDistance = Vector3.Distance(transform.position, target.transform.position);
    }

    /// <summary>
    /// Starts destruction sequence for clone.
    /// </summary>
    void Die()
    {
        _dying = true;
        _healthBar.gameObject.SetActive(false);
        Hero.SetActive(false);
        _destroyEffect.Play();
    }

    /// <summary>
    /// Searches nearest enemy in range.
    /// </summary>
    void SearchTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10);
        foreach (var collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                EnemyBase enemy = collider.gameObject.GetComponent<EnemyBase>();
                if (enemy.Freeze)
                    continue;


                if (_target == null)
                {
                    SetTarget(collider.gameObject);
                }
                else
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (_targetDistance < distance)
                    {
                        SetTarget(collider.gameObject);
                    }
                }
            }
        }
    }

    bool ListenPause()
    {
        if (GameStateManager.Instance.GameLoop.Paused && !_paused)
        {
            _animator.speed = 0;
            _healthBar.gameObject.SetActive(false);
            _paused = true;
        }
        else if (!GameStateManager.Instance.GameLoop.Paused && _paused)
        {
            _animator.speed = 1;
            _healthBar.gameObject.SetActive(true);
            _paused = false;
        }

        return _paused;
    }
}
