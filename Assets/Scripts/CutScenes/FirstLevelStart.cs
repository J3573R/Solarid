using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstLevelStart : MonoBehaviour {

    public AnimationClip CameraAnimation;
    public Image TutorialMessage;

    private CameraFollow _cameraScript;
    private Animation _animation;
    private Image _blackScreen;
    private bool _animationCompleted;
    

	// Use this for initialization
	void Start () {
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _blackScreen.CrossFadeAlpha(0, 5, true);
        GameStateManager.Instance.GameLoop.Pause(false, false);
        _cameraScript = FindObjectOfType<CameraFollow>();
        _animation = _cameraScript.GetComponent<Animation>();
        TutorialMessage.enabled = false;
        _cameraScript.StopNormalCameraMovement = true;
        _cameraScript.AnimationComponent = this;
        _animation.clip = CameraAnimation;

        _animation.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (_animationCompleted)
        {
            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)) {
                GameStateManager.Instance.GameLoop.UnPause();
                TutorialMessage.enabled = false;
                Destroy(gameObject);
                
            }
        }
	}

    public void AnimationCompleted()
    {
        _animation.Stop();
        _cameraScript.ResetCamera(false);
        TutorialMessage.enabled = true;
        _animationCompleted = true;
        
    }
}
