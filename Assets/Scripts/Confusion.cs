using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : MonoBehaviour
{

    public float Duration;
    public float Increment;

    private float time = 0;

	void Update () {

	    if (Duration > time)
	    {
	        transform.localScale += Vector3.one*Increment*Time.deltaTime;
	        transform.Rotate(0, -5, 2);
	        time += Time.deltaTime;
	    }
	    else
	    {
	        Destroy(gameObject);
	    }

	}
}
