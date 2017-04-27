using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class South5Start : MonoBehaviour {

    public AnimationClip CameraAnimation;
    public Transform CameraPos;

    private CameraFollow _cameraScript;
    private Animation _animation;
    private Image _blackScreen;
    private Transform _playerTransform;
    private PlayerMovement _playerMovement;
    private bool _animationCompleted;

    // Use this for initialization
    void Start()
    {
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _blackScreen.CrossFadeAlpha(0, 4, true);
        GameStateManager.Instance.GameLoop.Pause(false, false);
        _cameraScript = FindObjectOfType<CameraFollow>();
        _cameraScript.Init();
        _animation = _cameraScript.GetComponent<Animation>();
        _playerTransform = FindObjectOfType<Player>().transform;
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _playerMovement.GetComponent<InputController>().CinematicMovement = true;
        _cameraScript.StopNormalCameraMovement = true;
        _cameraScript.AnimationComponent = this;
        _animation.clip = CameraAnimation;
        _animation.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_animationCompleted)
        {
            if (_playerTransform.position.z < 0)
                _playerMovement.Move(0, 1);            
            else
                _playerMovement.Move(0, 0);
        }       
    }

    public void AnimationCompleted()
    {
        _playerMovement.GetComponent<InputController>().CinematicMovement = false;
        _blackScreen.CrossFadeAlpha(1, 2, true);        
        _animation.Stop();
        _animationCompleted = true;
        StartCoroutine(ToGameTransition());                        
    }

    private IEnumerator ToGameTransition()
    {
        yield return new WaitForSeconds(2);
        _cameraScript.transform.position = CameraPos.position;
        _cameraScript.transform.rotation = CameraPos.rotation;
        _cameraScript.ResetCamera(true);
        _blackScreen.CrossFadeAlpha(0, 2, true);
        GameStateManager.Instance.GameLoop.Paused = false;
        _playerTransform.GetComponent<InputController>().CinematicMovement = false;
    }
}
