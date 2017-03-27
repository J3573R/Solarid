using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour {

    public float Lifetime;
    public GameObject Hero;

    private Gun _gun;
    private float _targetDistance = Mathf.Infinity;
    private GameObject _target;
    private Animator _animator;
    private float _lifetime;
    private ParticleSystem _destroyEffect;
    private bool _dying;   
    
    void Awake()
    {
        _gun = GetComponentInChildren<Gun>();
        _animator = GetComponentInChildren<Animator>();
        _destroyEffect = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {                
        _lifetime = Lifetime;        
        _destroyEffect.Stop();
        Hero.SetActive(true);
        _dying = false;
    }
		
	void Update () {

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
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = lookRotation;//Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
                _gun.ShootDirection(_target.transform.position);
            }
            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0)
            {
                _dying = true;
                Hero.SetActive(false);
                _destroyEffect.Play();
            }
        } else
        {
            if (!_destroyEffect.IsAlive())
            {
                gameObject.SetActive(false);
            }            
        }
        

        
    }
}
