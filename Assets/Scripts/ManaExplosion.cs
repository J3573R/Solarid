using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaExplosion : MonoBehaviour {

    public GameObject ManaGlobePrefab;

    // Pool settings
    private const int _poolStartSize = 100;
    private const int _defaultGlobeAmount = 5;
    private GameObject _container;

    private List<GameObject> _pool = new List<GameObject>();

    void Awake()
    {
        _container = new GameObject("ManaPool");        
        for(int i = 0; i < _poolStartSize; i++)
        {
            AddToPool();
        }
    }

    public void AddToPool()
    {
        ReturnToPool(Instantiate(ManaGlobePrefab, transform.position, Quaternion.identity));
    }

    public void ReturnToPool(GameObject mana)
    {
        mana.transform.parent =
            _container.transform;
        mana.SetActive(false);
        _pool.Add(mana);
    }

    public GameObject GetFromPool()
    {
        GameObject result = null;
        if(_pool.Count == 0)
        {
            for(int i = 0; i < 5; i++)
            {
                Debug.Log("CREATING NEW");
                AddToPool();
            }            
        }

        foreach (var obj in _pool)
        {
            result = obj;
            _pool.Remove(obj);
            break;
        }

        return result;
    }
    
	public void Explode(Vector3 location, int globeAmount = _defaultGlobeAmount)
    {
        for(int i = 0; i < globeAmount; i++)
        {
            GameObject result = null;
            while(result == null)
            {
                result = GetFromPool();
            }
            result.transform.position = location;
            result.SetActive(true);
        }
        
    }
}
