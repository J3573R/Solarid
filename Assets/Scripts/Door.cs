using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Direction which door starts to open
    public Vector3 OpenDirection = new Vector3(0, -7.8f, 0);
    public AudioClip AudioRising;

    private float _speed = 2;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _state = 0;
    // Is door currently moving
    private bool _moving = false;
    // State of the door
    private bool _open = false;
    private AudioSource _audio;

    public bool Open { get { return _open; } }
    public bool Moving { get { return _moving; } }

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }


    /// <summary>
    /// Sets door current position and target position & starts enables moving.
    /// </summary>
    /// <returns>True if successfully started moving door, otherwise false</returns>
    public bool ToggleDoor()
    {
        if (!_moving)
        {
            if (!_open)
            {
                _endPosition = transform.position + OpenDirection;
            }
            else
            {
                _endPosition = transform.position - OpenDirection;
            }

            _startPosition = transform.position;
            _open = !_open;
            _state = 0;
            GameStateManager.Instance.GameLoop.References.CameraScript.Harlem(0.2f, 2f);
            _moving = true;
            if (_audio != null)
            {
                _audio.clip = AudioRising;
                _audio.volume = 0;
                _audio.Play();
            }
            
            return true;
        }

        return false;
    }
    
    void Update()
    {
        if (_moving && _state < 1)
        {
            _state += Time.deltaTime / _speed;
            
            AudioFade();
            
            transform.position = Vector3.Lerp(_startPosition, _endPosition, Easing.EaseInOut(_state, EasingType.Quartic, EasingType.Quartic));
            if (_state >= 1)
            {
                if (_audio != null)
                {
                    _audio.Stop();
                }
                    
                _moving = false;
                _state = 1;
            }
        }
    }

    void AudioFade()
    {
        if(_audio == null)
            return;

        // Audio fade effect by Tommi
        if (_state / 0.5f <= 1)
        {
            //_audio.volume += Mathf.Lerp(0, 1, Easing.EaseIn(_state / 0.5f, EasingType.Quadratic));
            _audio.volume += Time.deltaTime;
        }
        else
        {
            //_audio.volume -= Mathf.Lerp(1, 0, Easing.EaseOut(_state / 0.5f - 1 / 0.5f, EasingType.Quadratic));
            _audio.volume -= Time.deltaTime;
        }
    }

}
