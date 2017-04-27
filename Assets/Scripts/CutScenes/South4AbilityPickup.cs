using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class South4AbilityPickup : MonoBehaviour {

    public Transform PlayerStartPosition;
    public Transform PlayerEndPosition;

    private bool _play = false;
    private bool _playerStopped = false;
    private bool _playerAnimationPlayed = false;
    private bool _finished = false;
    private Image _blackScreen;
    private CameraFollow _mainCamera;
    private Animation _cameraAnimation;
    private Player _player;
    private PlayerAnimation _playerAnimation;
    private PlayerMovement _playerMovement;
    private AbilityController _abilityController;
    private InputController _inputController;
    private GameObject _hud;

    void Awake()
    {
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();        
        _mainCamera = FindObjectOfType<CameraFollow>();
        _player = FindObjectOfType<Player>();
        _playerAnimation = FindObjectOfType<PlayerAnimation>();
        _inputController = _player.GetComponent<InputController>();
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _abilityController = _player.GetComponent<AbilityController>();
        _cameraAnimation = _mainCamera.gameObject.GetComponent<Animation>();
        _hud = GameObject.Find("HUD");
    }

    void Update()
    {
        if (_play)
        {

            var dir = transform.position - PlayerEndPosition.position;
            if(_player.transform.position.x < PlayerEndPosition.position.x)
            {
                _playerMovement.Move(1, 0);
            } else
            {
                _playerMovement.Move(0, 0);
                _player.transform.LookAt(PlayerEndPosition);
                _playerStopped = true;
            }

            if (!_playerAnimationPlayed && _playerStopped && !_cameraAnimation.isPlaying)
            {
                _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.Praise);
                _playerAnimationPlayed = true;
                StartCoroutine(FadeOut());
            }            

            if (_finished)
            {
                _abilityController.EnableOrDisableAbility(AbilityController.Ability.Blink, true);
                SetCinematicMode(false);
                gameObject.SetActive(false);
            }
        }        
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3);
        _blackScreen.CrossFadeAlpha(1, 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        _hud.SetActive(true);
        _blackScreen.CrossFadeAlpha(0, 0.5f, true);        
        _finished = true;
    }

    private IEnumerator FadeIn()
    {
        _blackScreen.CrossFadeAlpha(1, 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        _hud.SetActive(false);
        SetCinematicMode(true);
        GameStateManager.Instance.GameLoop.References.Player.transform.position = PlayerStartPosition.position;
        _cameraAnimation.Play();
        _blackScreen.CrossFadeAlpha(0, 0.5f, true);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(FadeIn());
        }             
    }

    void SetCinematicMode(bool active)
    {        
        _play = active;
        _inputController.CinematicMovement = active;
        GameStateManager.Instance.GameLoop.References.CameraScript.StopNormalCameraMovement = active;
        if (active)
        {
            GameStateManager.Instance.GameLoop.Pause(false, false);
        } else
        {
            GameStateManager.Instance.GameLoop.References.CameraScript.ResetCamera();
            GameStateManager.Instance.GameLoop.UnPause();
        }
    }
	
}
