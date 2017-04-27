﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class West5Crystal : MonoBehaviour
{
    /*
    [SerializeField]
    private List<GameObject> _chargers;
    private List<Health> _healths;
    */

    [SerializeField]
    private Transform _playerPos;
    [SerializeField]
    private Transform _cameraPos;
    [SerializeField]
    private AnimationClip _animationClip;

    private BoxCollider _collider;
    private BoxCollider _endCollider;
    private Player _player;
    private Animation _animation;
    private MeshRenderer _crystalRenderer;
    private ParticleSystem _crystalParticle;
    private Light _crystalLight;
    private Door _door;
    private GameObject _hud;
    private AbilityBlink _blink;

    private CameraFollow _cameraScript;
    private bool _cutSceneEnabled;
    private Image _blackScreen;

    // Use this for initialization
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _player = FindObjectOfType<Player>();
        _cameraScript = FindObjectOfType<CameraFollow>();
        _blink = FindObjectOfType<AbilityBlink>();
        _animation = GameObject.Find("CrystalRed").GetComponent<Animation>();
        _crystalRenderer = GameObject.Find("CrystalRed").GetComponent<MeshRenderer>();
        _crystalParticle = GameObject.Find("RedPower").GetComponent<ParticleSystem>();
        _crystalLight = GameObject.Find("RedLight").GetComponent<Light>();
        _hud = GameObject.Find("HUD");

        /*
        _healths = new List<Health>();
        GetEnemies();
        */
    }

    /*
    private void GetEnemies()
    {
        foreach (GameObject charger in _chargers)
        {
            _healths.Add(charger.GetComponent<Health>());
        }
    }
	*/

    // Update is called once per frame
    void Update()
    {
        //CheckEnemiesAlive();
    }

    /*
    private void CheckEnemiesAlive()
    {
        if (!_collider.enabled)
        {
            int tmp = 0;
            foreach (Health hp in _healths)
            {
                if (!hp.IsDead())
                    tmp += 1;

                Debug.Log(tmp);
            }

            if (tmp == 0)
            {
                _collider.enabled = true;
            }
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PrepareCutScene();
            _cutSceneEnabled = true;
            _collider.enabled = false;
        }
    }

    private void PrepareCutScene()
    {
        GameStateManager.Instance.GameLoop.Pause(false, false);

        _blackScreen.CrossFadeAlpha(1, 2, true);
        StartCoroutine(StartCutScene());
    }

    private IEnumerator StartCutScene()
    {
        yield return new WaitForSeconds(2);

        Physics.OverlapSphere(_playerPos.position, 5f);
        _hud.SetActive(false);
        _cameraScript.StopNormalCameraMovement = true;        
        _player.transform.position = new Vector3(_playerPos.position.x, _player.transform.position.y, _playerPos.position.z);
        _player.transform.rotation = Quaternion.identity;
        _player.Input.CinematicMovement = true;
        _cameraScript.transform.position = _cameraPos.position;
        _cameraScript.transform.rotation = _cameraPos.rotation;
        _blackScreen.CrossFadeAlpha(0, 2, true);
        StartCoroutine(Forward1());
    }

    private IEnumerator Forward1()
    {
        yield return new WaitForSeconds(2);
        _animation.clip = _animationClip;
        _animation.Play();
        StartCoroutine(Forward2());
    }

    private IEnumerator Forward2()
    {
        yield return new WaitForSeconds(2);
        _crystalRenderer.enabled = false;
        _crystalParticle.Stop();
        _crystalLight.enabled = false;
        StartCoroutine(Forward3());

    }

    private IEnumerator Forward3()
    {
        yield return new WaitForSeconds(2);
        _player.Movement.SetCasting(true);
        _blink.BlinkToNothing();
        _blackScreen.CrossFadeAlpha(1, 2, true);
        StartCoroutine(Forward4());
        
    }

    private IEnumerator Forward4()
    {
        yield return new WaitForSeconds(2);        
        SaveSystem.Instance.SaveData.SetCrystals(SaveData.Crystal.Red, SaveSystem.Instance.SaveData.GetHubCrystals());
        SaveSystem.Instance.SaveData.SetHubState(SaveData.HubState.BlueActivated);

        try
        {
            SaveSystem.Instance.SaveAll();
            GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, "Hub");
        }
        catch (Exception e)
        {
            Debug.Log("ERROR CATCHED:");
            Debug.LogError(e.Message);
        }
    }

}
