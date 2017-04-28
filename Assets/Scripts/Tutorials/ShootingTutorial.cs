using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingTutorial : MonoBehaviour {
        
    private BoxCollider _collider;
    private Image _tutorial;
    private bool _tutorialActive;
    private Player _player;
    private Text _bulletsRemaining;    

	// Use this for initialization
	void Start () {
        _tutorial = GameObject.Find("ShootingTutorial").GetComponent<Image>();
        _player = FindObjectOfType<Player>();
        _tutorial.CrossFadeAlpha(0, 0, true);
        _bulletsRemaining = GameObject.Find("BulletsRemaining").GetComponent<Text>();
        _collider = GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update () {
		if (_tutorialActive)
        {
            if (Input.GetButton("Fire1"))
            {
                _player.Gun.ForceShoot();
                _tutorial.CrossFadeAlpha(0, 0, true);
                GameStateManager.Instance.GameLoop.UnPause();
                
                StartCoroutine(DestroyDelay());
                _tutorialActive = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _bulletsRemaining.CrossFadeAlpha(1, 1, true);
            _collider.enabled = false;
            GameStateManager.Instance.GameLoop.Pause(false, false);
            _tutorialActive = true;
            _tutorial.CrossFadeAlpha(1, 1, true);
            _player.Input.ShootingDisabled = false;            
        }
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
