using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVortex : AbilityBase
{
    public int PoolSize = 3;

    [SerializeField]
    private GameObject _blast;
    private Player _player;
    private Vector3 _target;
    private List<GameObject> _vortexPool = new List<GameObject>();

    private void Start()
    {
        _player = GetComponent<Player>();
        InitPool();
    }

    /// <summary>
    /// Fills pool with vortex prefabs.
    /// </summary>
    private void InitPool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            AddToPool();
        }
    }

    /// <summary>
    /// Adds one gameobject to vortex pool.
    /// </summary>
    private void AddToPool()
    {
        GameObject temp = Instantiate(_blast, _target, Quaternion.Euler(90, 0, 0));
        temp.SetActive(false);
        _vortexPool.Add(temp);
    }

    /// <summary>
    /// Returns vortex into the object pool.
    /// </summary>
    /// <param name="vortex">Vortex gameobject</param>
    public void ReturnToPool(GameObject vortex)
    {
        vortex.SetActive(false);
        _vortexPool.Add(vortex);
    }

    /// <summary>
    /// Returns vortex from gameobject pool.
    /// </summary>
    /// <returns>Instantiated and activated vortex prefab</returns>
    public GameObject GetFromPool()
    {
        GameObject result = null;

        if (_vortexPool.Count > 0)
        {
            result = _vortexPool[0];
            _vortexPool.RemoveAt(0);
        }
        else
        {
            AddToPool();
            result = _vortexPool[0];
            _vortexPool.RemoveAt(0);
        }

        result.SetActive(true);
        return result;
    }

    /// <summary>
    /// Gets the mouseposition and throws the grenade to it
    /// </summary>
    public override void Execute(Vector3 targetPos)
    {
        _target = targetPos;
        _player.Animation.CastOnce = true;
        StartCoroutine(CastDelay());
    }

    /// <summary>
    /// Delay before the actual execution so animation can complete
    /// </summary>
    /// <returns></returns>
    private IEnumerator CastDelay()
    {
        yield return new WaitForSeconds(_player.AbilityController.CastDelayInSeconds);
        if (_target != Vector3.zero)
        {
            _target.y = 1;
            Instantiate(_blast, _target, Quaternion.Euler(90, 0, 0));
        }
    }

}
