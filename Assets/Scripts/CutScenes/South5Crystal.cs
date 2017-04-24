using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class South5Crystal : MonoBehaviour {

    [SerializeField]
    private GameObject[] _chargers;

    private BoxCollider _collider;    
    private BoxCollider _endCollider;
    private Health[] _healths;
    private bool _cutSceneEnabled;
    private Image _blackScreen;

	// Use this for initialization
	void Start () {
        _collider = GetComponent<BoxCollider>();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        GameStateManager.Instance.GameLoop.Pause(false, false);
        
        
    }

    private void GetEnemies()
    {
        int index = 0;

        foreach (GameObject charger in _chargers)
        {
            _healths[index] = charger.GetComponent<Health>();
            index += 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
        CheckEnemiesAlive();
	}

    private void CheckEnemiesAlive()
    {
        if (!_collider.enabled)
        {
            int tmp = 0;
            foreach (Health hp in _healths)
            {
                if (!hp.IsDead())
                    tmp += 1;
            }

            if (tmp == 0)
            {
                _collider.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PrepareCutScene();
            _cutSceneEnabled = true;
        }
    }

    private void PrepareCutScene()
    {
        Debug.Log("Preparing So hard");
        _blackScreen.CrossFadeAlpha(1, 2, true);
    }
}
