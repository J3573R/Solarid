using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class South5Crystal : MonoBehaviour {
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
    private CameraFollow _cameraScript;
    private bool _cutSceneEnabled;
    private Image _blackScreen;


    // Use this for initialization
    void Start() {
        _collider = GetComponent<BoxCollider>();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _player = FindObjectOfType<Player>();
        _cameraScript = FindObjectOfType<CameraFollow>();
        _animation = GameObject.Find("CrystalBlue").GetComponent<Animation>();
        _crystalRenderer = GameObject.Find("CrystalBlue").GetComponent<MeshRenderer>();
        _crystalParticle = GameObject.Find("BluePower").GetComponent<ParticleSystem>();
        _crystalLight = GameObject.Find("BlueLight").GetComponent<Light>();
        _door = GameObject.Find("Rising Platform Cutscene").GetComponent<Door>();
		_hud = GameObject.Find("HUD");

    }

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
        _blackScreen.CrossFadeAlpha(1, 2, true);
        StartCoroutine(Forward4());
    }

    private IEnumerator Forward4()
    {
        yield return new WaitForSeconds(2);
        _cameraScript.ResetCamera(true);
        GameStateManager.Instance.GameLoop.Paused = false;
        _player.Input.CinematicMovement = false;
        _blackScreen.CrossFadeAlpha(0, 2, true);
        SaveSystem.Instance.SaveData.SetCrystals(SaveData.Crystal.Blue, SaveSystem.Instance.SaveData.GetHubCrystals());
        SaveSystem.Instance.SaveData.SetHubState(SaveData.HubState.NothingActivated);
        _door.ToggleDoor();
    }

}
