using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPool : MonoBehaviour
{
    public GameObject ManaPrefab;
    public int ManaAmount = 5;

    private GameObject _container;
    private List<GameObject> _pool = new List<GameObject>();

	void Awake ()
	{
	    _container = GameObject.Find("ManaPool");

	    if (_container == null)
	    {
	        _container = new GameObject("ManaPool");
	    }

	    for (int i = 0; i < ManaAmount; i++)
	    {
	        GameObject tmp = Instantiate(ManaPrefab, transform.position, Quaternion.identity);
	        tmp.transform.parent = _container.transform;
            tmp.SetActive(false);
            _pool.Add(tmp);
	    }
	}

    public void Explode(Vector3 location)
    {
        foreach (var mana in _pool)
        {
            mana.transform.position = location;
            mana.SetActive(true);
        }
    }
}
