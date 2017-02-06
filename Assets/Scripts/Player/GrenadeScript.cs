using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

    private MeshRenderer _renderer;
    public  Vector3 targetPosition;

    public  float angle;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Throw()
    {
        //Vector3 velocity = 
    }
}
