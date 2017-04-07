using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaExplosion : MonoBehaviour {

    public GameObject ManaGlobePrefab;
    public int ManaAmount;
    
	void Explode()
    {
        for(int i = 0; i < ManaAmount; i++)
        {

        }

        gameObject.SetActive(false);
    }
}
