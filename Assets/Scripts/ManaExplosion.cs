using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaExplosion : MonoBehaviour {

    public GameObject ManaGlobePrefab;

    // Pool settings
    private const int _poolStartSize = 10;
    private const int _defaultGlobeAmount = 5;

    private List<GameObject> _pool = new List<GameObject>();
    private GameObject _poolObject;

    void Awake()
    {
        for(int i = 0; i < _poolStartSize; i++)
        {
            _poolObject = Instantiate(ManaGlobePrefab, transform.position, Quaternion.identity);
            AddToPool(_poolObject);
        }
        Globals.ManaExplosion = this;
    }

    public void AddToPool(GameObject gameobject)
    {
        gameobject.SetActive(false);
        _pool.Add(gameobject);
    }

    public GameObject GetFromPool()
    {
        if(_pool.Count > 0)
        {
            _poolObject = _pool[0];
        } else
        {
            for(int i = 0; i < 5; i++)
            {
                _poolObject = Instantiate(ManaGlobePrefab, transform.position, Quaternion.identity);
                AddToPool(_poolObject);
            }

            _poolObject = _pool[0];            
        }

        _pool.RemoveAt(0);
        return _poolObject;
    }
    
	public void Explode(Vector3 location, int globeAmount = _defaultGlobeAmount)
    {
        for(int i = 0; i < globeAmount; i++)
        {
            _poolObject = GetFromPool();
            _poolObject.transform.position = location;
            _poolObject.SetActive(true);
        }
        
    }
}
