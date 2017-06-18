using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class West5Start : MonoBehaviour {

    public AnimationClip CameraAnimation;
    public Transform CameraPos;
	public Image NoMana;

    private CameraFollow _cameraScript;
    private Animation _animation;
    private Image _blackScreen;
    private Transform _playerTransform;
    private PlayerMovement _playerMovement;
    private bool _animationCompleted;
	private GameObject _hud;

    // Use this for initialization
    void Start()
    {
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _blackScreen.CrossFadeAlpha(0, 4, true);
        GameStateManager.Instance.GameLoop.Pause(false, false);
        _cameraScript = FindObjectOfType<CameraFollow>();
        _cameraScript.Init();
		_hud = GameObject.Find("HUD");
		_hud.SetActive(false);
        _animation = _cameraScript.GetComponent<Animation>();
        _cameraScript.StopNormalCameraMovement = true;
        _cameraScript.AnimationComponent = this;
        _animation.clip = CameraAnimation;
        _animation.Play();
    }


    public void AnimationCompleted()
    {
        _blackScreen.CrossFadeAlpha(1, 2, true);
        _animation.Stop();
        _animationCompleted = true;
        StartCoroutine(ToGameTransition());
    }

    private IEnumerator ToGameTransition()
    {
        yield return new WaitForSeconds(2);
        //_cameraScript.transform.position = CameraPos.position;
        //_cameraScript.transform.rotation = CameraPos.rotation;
        _cameraScript.ResetCamera(true);
		_hud.SetActive(true);
		NoMana.CrossFadeAlpha(0, 0, true);
        _blackScreen.CrossFadeAlpha(0, 2, true);
        GameStateManager.Instance.GameLoop.Paused = false;
    }
}
