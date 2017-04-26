using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    // Bullet prefab
    public GameObject Bullet;
    // BulletInterval time of the gun in seconds
    public float BulletInterval;
    // Amount of bullets in pool
    public int PooledBullets;
    // pool of bullets
    public List<GameObject> _bullets;
    // Amount of bullets you can fire before reload
    public int ClipSize;
    // Bullets remaining in clip
    public int BulletsRemaining;
    // Reload time of the gun
    public float ReloadTime;
    public AudioClip AudioShot;
    public AudioClip AudioReload;
    public bool Initialized;

    public float TargetDistance;    
    public float RecoilBuildUp;
    public float MaxRecoil;
    public bool Shooting;
    public bool Reloading;

    private GameObject _container;
    private float _recoil;
    private Player _player;
    private Vector3 _target;
    private Vector3 _aimRotation;
    private Transform _aimPoint;
    
    // Current status of reload
    private float _intervalTimer = 0;
    private AudioSource _audio;    

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (!Initialized)
        {
            _container = new GameObject("PlayerBulletPool");
            BulletsRemaining = ClipSize;
            _player = FindObjectOfType<Player>();
            _aimPoint = GameObject.Find("AimPoint").transform;
            _audio = GetComponent<AudioSource>();
            SetupBulletPool();
            Initialized = true;
        }
    } 

    /// <summary>
    /// Sets up the object pool of bullets
    /// </summary>
    private void SetupBulletPool()
    {
        for (int i = 0; i < PooledBullets; i++)
        {
            GameObject tmpGO = Instantiate(Bullet, transform.position, Quaternion.identity) as GameObject;
            tmpGO.transform.parent = _container.transform;
            tmpGO.SetActive(false);
            _bullets.Add(tmpGO);
        }
    }

    /// <summary>
    /// Sets the status of shooting...
    /// </summary>
    /// <param name="state"></param>
    public void SetShooting(bool state)
    {
        if (state)
        {
            Shooting = state;
        }
        else
        {
            Shooting = false;
            _recoil = 0;
        }
    }

    void Update()
    {
        if (!GameStateManager.Instance.GameLoop.Paused)
        {
            if (_intervalTimer > 0)
            {
                _intervalTimer -= Time.deltaTime * 1;
            }

            if (!Shooting && _recoil > 0)
            {
                _recoil -= Time.deltaTime / 2;

            }

            if (BulletsRemaining <= 0 && !Reloading)
            {
                Reloading = true;
                _audio.clip = AudioReload;
                _audio.loop = true;
                _audio.Play();
                StartCoroutine(Reload());
            }
        }        
    }

    public void InitiateReload()
    {
        
        if (!Reloading && BulletsRemaining < ClipSize)
        {
            Reloading = true;
            _audio.clip = AudioReload;
            _audio.loop = true;
            _audio.Play();
            StartCoroutine(Reload());
        }        
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(ReloadTime);
        _audio.Stop();
        Reloading = false;
        BulletsRemaining = ClipSize;
    }

    public void ForceShoot()
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (!_bullets[i].activeInHierarchy)
            {
                _target = GetTargetPosition();
                _bullets[i].transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
                _target.y = 1.5f;
                _bullets[i].transform.LookAt(_target);
                _bullets[i].SetActive(true);
                BulletsRemaining -= 1;
                _audio.clip = AudioShot;
                _audio.loop = false;
                _audio.Play();
                break;
            }
        }
        _intervalTimer = BulletInterval;
    }

    /// <summary>
    /// Searches object pool for inactive bullet and fires it
    /// </summary>
    public void Shoot()
    {       
        if (_intervalTimer <= 0 && !Reloading && BulletsRemaining > 0)
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (!_bullets[i].activeInHierarchy)
                {
                    _target = GetTargetPosition();
                    _bullets[i].transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
                    _target.y = 1.5f;
                    _bullets[i].transform.LookAt(_target);
                    _bullets[i].SetActive(true);
                    BulletsRemaining -= 1;
                    _audio.clip = AudioShot;
                    _audio.loop = false;
                    _audio.Play();
                    break;
                }
            }
            _intervalTimer = BulletInterval;

            if (_recoil < MaxRecoil)
            {
                _recoil += RecoilBuildUp;
            }
        }        
    }

    

    /// <summary>
    /// Shoot function for the clone. Shoots the direction of the vector from transform.
    /// </summary>
    public void ShootDirection(Vector3 direction)
    {
        
        if (_intervalTimer <= 0 )
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (!_bullets[i].activeInHierarchy)
                {
                    _target = direction;
                    _bullets[i].transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
                    _target.y = 1.5f;
                    //Debug.Log(_target);
                    _bullets[i].transform.LookAt(_target);
                    _bullets[i].SetActive(true);
                    break;
                }
            }
            _intervalTimer = BulletInterval;
        }
    }

    /// <summary>
    /// Returns the targetposition where the player is aiming
    /// </summary>
    /// <returns></returns>
    private Vector3 GetTargetPosition()
    {
        Vector3 tmpVec = _player.Input.GetMousePosition();
        
        Vector3 PlayerPos = _player.transform.position;
        PlayerPos.y = 1.5f;

        Vector3 direction = (tmpVec - PlayerPos).normalized;
        float tmp = UnityEngine.Random.Range(-_recoil, _recoil);

        _aimPoint.position = transform.position + direction * TargetDistance;
        _aimPoint.rotation = transform.rotation;
        tmp = UnityEngine.Random.Range(-_recoil, _recoil);
        _aimPoint.position = _aimPoint.position + (_aimPoint.forward * tmp);

        return _aimPoint.position;

        // Here's a working alternative for recoil calculation, not optimal. Dont use
        /*
        Vector3 recoilPos = tmpPos;
        recoilPos.x += tmp;
        tmp = UnityEngine.Random.Range(-_recoil, _recoil);
        recoilPos.z += tmp;
        
        return recoilPos;
        */
    }
}
