using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DontGoHereMessage : MonoBehaviour {

    private Image _message;

	// Use this for initialization
	void Start () {
        _message = GameObject.Find("DontGoHere").GetComponent<Image>();
        _message.CrossFadeAlpha(0, 0, true);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _message.CrossFadeAlpha(1, 1, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _message.CrossFadeAlpha(0, 1, true);
        }
    }
}
