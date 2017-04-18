using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Vortex : MonoBehaviour
{

    public int DamagePerSecond = 50;
    public float Lifetime = 5f;

    private float _damageTickTime = 0;
    private ParticleSystem _particleSystem;
    private AudioSource _audio;
    private bool _paused = false;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
    }

	void Update () {

	    if (ListenPause())
	    {
	        return;
	    }

        if (_damageTickTime <= 0)
        {
            _damageTickTime = 1;
        }

        _damageTickTime -= Time.deltaTime;
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider enemy in enemyColliders)
        {
            EnemyBase tmp = enemy.gameObject.GetComponent<EnemyBase>();
            if (tmp != null)
            {
                tmp.PullToPoint(transform.position, 0.1f);

                if (_damageTickTime <= 0)
                {
                    tmp.TakeDamage(DamagePerSecond);
                }
            }
        }

        if (Lifetime <= 0)
        {
            Destroy(gameObject);
        }
        Lifetime -= Time.deltaTime;
    }

    bool ListenPause()
    {
        if (GameStateManager.Instance.GameLoop.Paused && !_paused)
        {
            _particleSystem.Pause(true);
            _audio.Pause();
            _paused = true;
        }
        else if (!GameStateManager.Instance.GameLoop.Paused && _paused)
        {
            _particleSystem.Play(true);
            _audio.Play();
            _paused = false;
        }

        return _paused;
    }
}
