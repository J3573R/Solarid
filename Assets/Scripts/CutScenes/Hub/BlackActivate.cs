using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackActivate : MonoBehaviour {

    [SerializeField]
    private GameObject _megaLaser;
    [SerializeField]
    private GameObject _rain;
    [SerializeField]
    private GameObject _playerPos;
    [SerializeField]
    private AnimationClip _firstAnimationClip;
    [SerializeField]
    private AnimationClip _secondAnimationClip;
    [SerializeField]
    private GameObject _blackCrystal;

	private AudioSource _audioSource;
    private CameraFollow _cameraScript;
    private Animation _cameraAnimation;
    private Image _blackScreen;
    private Player _player;
	private GameObject _hud;

	// Use this for initialization
	void Start () {
		_audioSource = GetComponent<AudioSource> ();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _blackScreen.CrossFadeAlpha(1, 2, true);
        _cameraAnimation = FindObjectOfType<Camera>().GetComponent<Animation>();
        _player = FindObjectOfType<Player>();
        GameStateManager.Instance.GameLoop.Pause(false, false);
        _cameraScript = FindObjectOfType<CameraFollow>();
        _cameraScript.StopNormalCameraMovement = true;
        StartCoroutine(StartCutScene());
		_hud = GameObject.Find("HUD");
	}

    private IEnumerator StartCutScene()
    {
        yield return new WaitForSeconds(2);
		_hud.SetActive (false);
        _player.transform.position = _playerPos.transform.position;
        _player.transform.rotation = _playerPos.transform.rotation;
        _cameraAnimation.clip = _firstAnimationClip;
        _cameraAnimation.Play();
        _blackScreen.CrossFadeAlpha(0, 2, true);

        StartCoroutine(ActivateCrystal());
    }

    private IEnumerator ActivateCrystal()
    {
        yield return new WaitForSeconds(3);
        _blackCrystal.SetActive(true);        
        StartCoroutine(Wait());        
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        _blackScreen.CrossFadeAlpha(1, 1, true);
        StartCoroutine(ChangeClip());
    }

    private IEnumerator ChangeClip()
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0.6f;
        _cameraAnimation.clip = _secondAnimationClip;
        _cameraAnimation.Play();
        _megaLaser.SetActive(true);
        _blackScreen.CrossFadeAlpha(0, 1, true);
		StartCoroutine (WaitMusic ());
        StartCoroutine(RainDelay());
    }

	private IEnumerator WaitMusic()
	{
		yield return new WaitForSeconds(2);
		_audioSource.Play ();
	}

    private IEnumerator RainDelay()
    {
        yield return new WaitForSeconds(8);
        _rain.SetActive(true);

        StartCoroutine(GoToCredits());
    }

    private IEnumerator GoToCredits()
    {
        yield return new WaitForSeconds(3);
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.Credits, "Credits");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
