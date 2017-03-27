using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Player _player;
    // Speed of the bullet
    public float Speed = 5f;
    // Range of the bullet
    public float Range = 1f;
    // How long the hit effect is displayed, set in editor
    public float HitEffectTime;

    // Maximium travelled distance
    private float _fMaxDist;
    // Lifetime of the bullet
    private float _time;
    private ParticleSystem _hitPlasma1;
    private ParticleSystem _hitPlasma2;
    private bool _active;
    private Transform _bulletPart;
    private Transform _bulletHit;         

    void Awake()
    {
        _fMaxDist = Range / Speed;        
        _time = 0;
        GetObjects();
    }

    /// <summary>
    /// Gets the used object references
    /// </summary>
    private void GetObjects()
    {
        _bulletPart = gameObject.transform.GetChild(0);
        _bulletHit = gameObject.transform.GetChild(1);
        
        _player = FindObjectOfType<Player>();
        _hitPlasma1 = _bulletHit.GetChild(0).GetComponent<ParticleSystem>();
        _hitPlasma2 = _bulletHit.GetChild(1).GetComponent<ParticleSystem>();
        _bulletHit.transform.gameObject.SetActive(false);

    }

    private void OnDisable()
    {
        //transform.position = _player.transform.position;
    }

    /// <summary>
    /// Called if the bullet hits something. Starts the hit effect sequence and disables the bullet itself
    /// </summary>
    public void BulletHit()
    {
        _bulletPart.transform.gameObject.SetActive(false);
        _bulletHit.transform.gameObject.SetActive(true);
        _hitPlasma1.Play();
        _hitPlasma2.Play();
        _active = false;
        StartCoroutine(HitEffectDelay());
    }

    /// <summary>
    /// Delay before the hit effect is disabled too
    /// </summary>
    /// <returns></returns>
    private IEnumerator HitEffectDelay()
    {
        yield return new WaitForSeconds(HitEffectTime);
        _hitPlasma1.Stop();
        _hitPlasma2.Stop();
        _bulletHit.transform.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _bulletPart.transform.gameObject.SetActive(true);
        _active = true;
        _time = 0;
    }

    void FixedUpdate()
    {
        if (_active)
        {
            if (_time > _fMaxDist)
            {
                gameObject.SetActive(false);
                _time = 0;
            }
            else
            {
                transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                _time += Time.deltaTime;
            }
        }      
    }  
}
