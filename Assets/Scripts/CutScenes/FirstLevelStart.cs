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
    public float WaitTime;
    

	// Use this for initialization
	void Start () {
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _blackScreen.CrossFadeAlpha(0, 5, true);
        GameStateManager.Instance.GameLoop.Pause(false, false);
        _cameraScript = FindObjectOfType<CameraFollow>();
        _animation = _cameraScript.GetComponent<Animation>();
        TutorialMessage.CrossFadeAlpha(0, 0, true);
        _cameraScript.StopNormalCameraMovement = true;
        _cameraScript.AnimationComponent = this;
        _animation.clip = CameraAnimation;
        _animation.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (_animationCompleted)
        {
            if (WaitTime <= 0)
            {
                if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
                {
                    GameStateManager.Instance.GameLoop.UnPause();
                    TutorialMessage.CrossFadeAlpha(0, 1, true);
                    Destroy(gameObject);
                }
            }
            WaitTime -= Time.deltaTime;
        }
	}

    public void AnimationCompleted()
    {
        _animation.Stop();
        _cameraScript.ResetCamera(false);
        TutorialMessage.CrossFadeAlpha(1, 1, true);
        _animationCompleted = true;
        
    }
}
