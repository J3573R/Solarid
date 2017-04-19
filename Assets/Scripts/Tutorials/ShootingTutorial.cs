using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingTutorial : MonoBehaviour {

    private Image _tutorial;
    private bool _tutorialActive;
    private Player _player;

	// Use this for initialization
	void Start () {
        _tutorial = GameObject.Find("ShootingTutorial").GetComponent<Image>();
        _player = FindObjectOfType<Player>();
        _tutorial.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (_tutorialActive)
        {
            if (Input.GetButton("Fire1"))
            {
                _player.Gun.ForceShoot();
                _tutorial.enabled = false;
                GameStateManager.Instance.GameLoop.UnPause();
                Destroy(gameObject);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameStateManager.Instance.GameLoop.Pause(false, false);
            _tutorialActive = true;
            _tutorial.enabled = true;
        }
    }
}
