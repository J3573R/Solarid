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
    private Animator _animator;
    private float _lifetime;
    private ParticleSystem _destroyEffect;
    private ParticleSystem _circleParticle;
    
    private bool _dying;
    private Slider _healthBar;
    private bool _showHealth = false;

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
		
	void Update () {

        if (!GameStateManager.Instance.GameLoop.Paused)
        {
            _healthBar.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + HealthBarOffset);
            _healthBar.value = Health.CurrentHealth;

            UpdateLifeTimeCircle();

            if (!_showHealth && Health.CurrentHealth < _healthBar.maxValue)
            {
                _healthBar.gameObject.SetActive(true);
                _showHealth = true;
            }

            if (!_dying)
            {
                if (_target == null)
                {
                    _animator.SetInteger("animState", 0);
                    Collider[] colliders = Physics.OverlapSphere(transform.position, 10);
                    foreach (var collider in colliders)
                    {
                        if (collider.tag == "Enemy")
                        {
                            if (_target == null)
                            {
                                _target = collider.gameObject;
                            }
                            else
                            {
                                float distance = Vector3.Distance(transform.position, collider.transform.position);
                                if (_targetDistance < distance)
                                {
                                    _target = collider.gameObject;
                                    _targetDistance = distance;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _animator.SetInteger("animState", 1);
                    Vector3 direction = _target.transform.position - transform.position;
                    direction.y = 0;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = lookRotation;//Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
                    _gun.ShootDirection(_target.transform.position);
                }
                _lifetime -= Time.deltaTime;
                if (_lifetime <= 0 || Health.CurrentHealth <= 0)
                {
                    _dying = true;
                    _healthBar.gameObject.SetActive(false);
                    Hero.SetActive(false);
                    _destroyEffect.Play();
                }
            }
            else
            {
                if (!_destroyEffect.IsAlive())
                {
                    gameObject.SetActive(false);
                }
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
}
