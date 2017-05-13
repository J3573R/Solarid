using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CreditScroll : MonoBehaviour
{
    public bool MainMenuOnDestroy = false;

    private Collider _fadeIn;
    private Collider _fadeOut;
    private Text _text;
    private Collider _collider;
    private int _pressCount;

    void Awake()
    {
        _fadeIn = GameObject.Find("FadeIn").GetComponent<Collider>();
        _fadeOut = GameObject.Find("FadeOut").GetComponent<Collider>();
        _text = GetComponent<Text>();
        _text.CrossFadeAlpha(0, 0, false);
        _collider = GetComponent<Collider>();
    }

    void Update()
    {
        transform.Translate(Vector3.up);
        if (Input.anyKeyDown)
        {
            _pressCount++;
            if (_pressCount == 2)
            {
                GameObject.Find("Game State: Credits").GetComponent<StateCredits>().End();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.Equals(_fadeOut))
        {
            End();
        } else if (other.Equals(_fadeIn))
        {
            _text.CrossFadeAlpha(1, 2, false);
        }
    }

    public void End()
    {
        _text.CrossFadeAlpha(0, 1.9f, false);
        _collider.enabled = false;
        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(1.9f);
        if (MainMenuOnDestroy) GameObject.Find("Game State: Credits").GetComponent<StateCredits>().End();
        Destroy(gameObject);
    }
}
