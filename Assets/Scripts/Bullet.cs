using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Speed of the bullet
    public float Speed = 5f;
    // Range of the bullet
    public float Range = 1f;

    public float HitEffectTime;

    // Maximium travelled distance
    private float _fMaxDist;
    // Lifetime of the bullet
    private float _time;
    private ParticleSystem _hitPlasma1;
    private ParticleSystem _hitPlasma2;

    private Transform _bulletPart;
    private Transform _bulletHit;
    


    public Player _player;
    private bool _active;

    void Awake()
    {
        _fMaxDist = Range / Speed;
        
        _time = 0;

        GetObjects();
    }

    private void GetObjects()
    {
        _bulletPart = gameObject.transform.GetChild(0);
        _bulletHit = gameObject.transform.GetChild(1);

        //Debug.Log("BulletHit = " +_bulletHit.transform.name);
        //Debug.Log("BulletPart = " + _bulletPart.transform.name);
        
        _player = FindObjectOfType<Player>();
        _hitPlasma1 = _bulletHit.GetChild(0).GetComponent<ParticleSystem>();
        _hitPlasma2 = _bulletHit.GetChild(1).GetComponent<ParticleSystem>();
        _bulletHit.transform.gameObject.SetActive(false);

    }

    private void OnDisable()
    {
        //transform.position = _player.transform.position;
    }

    public void BulletHit()
    {
        _bulletPart.transform.gameObject.SetActive(false);
        _bulletHit.transform.gameObject.SetActive(true);
        _hitPlasma1.Play();
        _hitPlasma2.Play();
        _active = false;
        StartCoroutine(HitEffectDelay());
    }

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

    void Update()
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

        if (transform.position.y > 1.55f || transform.position.y < 1.45f)
        {
            Debug.Log("yli tai ali");
        }
        
    }

}
