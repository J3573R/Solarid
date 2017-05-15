using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTutorial : MonoBehaviour {

    private BoxCollider _collider;
    private Image _aim;
    private Image _cast;
    private Text _plus;
    private bool _tutorialActive;
    private Player _player;

    // Use this for initialization
    void Start()
    {
        _aim = GameObject.Find("Aim").GetComponent<Image>();
        _cast = GameObject.Find("Cast").GetComponent<Image>();
        _plus = GameObject.Find("Plus").GetComponent<Text>();
        _player = FindObjectOfType<Player>();
        _aim.CrossFadeAlpha(0, 0, true);
        _cast.CrossFadeAlpha(0, 0, true);
        _plus.CrossFadeAlpha(0, 0, true);
        _collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_tutorialActive)
        {
            if (Input.GetButton("Ability") && Input.GetButtonDown("Fire1"))
            {
                _aim.CrossFadeAlpha(0, 1, true);
                _cast.CrossFadeAlpha(0, 1, true);
                _plus.CrossFadeAlpha(0, 1, true);
                //GameStateManager.Instance.GameLoop.UnPause();                
                StartCoroutine(DestroyDelay());
                _tutorialActive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _collider.enabled = false;
            //GameStateManager.Instance.GameLoop.Pause(false, false);      
            _plus.CrossFadeAlpha(1, 1, true);      
            _aim.CrossFadeAlpha(1, 1, true);
            _cast.CrossFadeAlpha(1, 1, true);
            StartCoroutine(TutorialDelay());
        }
    }

    private IEnumerator TutorialDelay()
    {
        yield return new WaitForSeconds(1);
        _tutorialActive = true;        
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
