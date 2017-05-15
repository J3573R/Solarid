using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableShootingTutorial : MonoBehaviour {

    public GameObject ShootingTutorial;

    private Image _tutorial;    

    // Use this for initialization
    void Start () {
        _tutorial = GameObject.Find("ShootingTutorial").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _tutorial.CrossFadeAlpha(0, 1, true);
            StartCoroutine(DestroyDelay());
        }
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(1);
        Destroy(ShootingTutorial);
        Destroy(gameObject);
    }
}
