using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstLevelStart : MonoBehaviour {

    public AnimationClip CameraAnimation;
    public Image TutorialMessage;

    private CameraFollow _cameraScript;
    private Animation _animation;
    private Player _player;
    private PlayerHealth _playerHealth;
    private Image _blackScreen;
    private Image[] _hud;
    private Text _hudText;
    private bool _animationCompleted;
    private bool _tutorialCompleted;
    public float WaitTime;
    private bool _giveHp;

	// Use this for initialization
	void Start () {
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _blackScreen.CrossFadeAlpha(0, 5, true);
        GameStateManager.Instance.GameLoop.Pause(false, false);
        _cameraScript = FindObjectOfType<CameraFollow>();
        _animation = _cameraScript.GetComponent<Animation>();
        TutorialMessage.CrossFadeAlpha(0, 0, true);
        _hud = GameObject.Find("HUD").GetComponentsInChildren<Image>();
        _hudText = GameObject.Find("BulletsRemaining").GetComponent<Text>();
        _player = FindObjectOfType<Player>();
        _player.Input.ShootingDisabled = true;
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _playerHealth.TakeDamage(999);
        _player.Mana.SubStractMana(1000);
        
        foreach (Image img in _hud)
        {
            img.CrossFadeAlpha(0, 0, true);
        }
        _hudText.CrossFadeAlpha(0, 0, true);

        _cameraScript.StopNormalCameraMovement = true;
        _cameraScript.AnimationComponent = this;
        _animation.clip = CameraAnimation;
        _animation.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (_animationCompleted && !_tutorialCompleted)
        {
            if (WaitTime <= 0)
            {
                if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
                {
                    GameStateManager.Instance.GameLoop.UnPause();
                    TutorialMessage.CrossFadeAlpha(0, 0.5f, true);
                    StartCoroutine(HUDDelay());
                    _tutorialCompleted = true;
                }
            }
            WaitTime -= Time.deltaTime;
        }

        if (_giveHp)
        {
            _playerHealth.AddHealth(10);
            _player.Mana.AddMana(10);
            
            if (_playerHealth.CurrentHealth >= 1000)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            _player.Mana.SubStractMana(1);
        }
	}

    public void AnimationCompleted()
    {
        _animation.Stop();
        _cameraScript.ResetCamera(false);
        TutorialMessage.CrossFadeAlpha(1, 1, true);
        _animationCompleted = true;
        
    }

    private IEnumerator HUDDelay()
    {
        yield return new WaitForSeconds(1);
        foreach (Image img in _hud)
        {
            StartCoroutine(HpDelay());
            img.CrossFadeAlpha(1, 1, true);

            if (img.name.Equals("HudNoMana"))
            {
                img.CrossFadeAlpha(0,0, true);
            }            
        }        
    }

    private IEnumerator HpDelay()
    {
        yield return new WaitForSeconds(1);
        _giveHp = true;
    }
}
