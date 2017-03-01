using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerBulletPool : MonoBehaviour
{

    public GameObject BulletPrefab;
    public int PoolSize = 20;
    public int PoolIncrement = 5;

    private List<GameObject> _bulletPool;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        _bulletPool = new List<GameObject>();
        AddToPool(PoolSize);
    }

    void IncreasePool()
    {
        if (PoolIncrement > 0)
        {
            AddToPool(PoolIncrement);
        }
    }

    void AddToPool(int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            GameObject addToPool = Instantiate(BulletPrefab);
            addToPool.SetActive(false);
            _bulletPool.Add(addToPool);
        }
        
    }

    public GameObject GetBullet()
    {
        GameObject ret = null;

        if (_bulletPool.Count == 0)
        {
            AddToPool(PoolIncrement);
        }

        if (_bulletPool[0] != null)
        {
            ret = _bulletPool[0];
            _bulletPool.Remove(ret);
            ret.SetActive(true);
        }
        else
        {
            Debug.LogError("Enemy bullet pool is depleted.");
        }

        return ret;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        _bulletPool.Add(bullet);
    }
}
