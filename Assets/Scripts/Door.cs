using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 OpenDirection = new Vector3(0, -8, 0);

    private float _speed = 2;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _state = 0;
    private bool _moving = false;
    private bool _open = false;

    public bool Open { get { return _open; } }
    public bool Moving { get { return _moving; } }

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
            _moving = true;
            return true;
        }

        return false;
    }
    
    void Update()
    {

        if (_moving && _state < 1)
        {
            _state += Time.deltaTime / _speed;
            transform.position = Vector3.Lerp(_startPosition, _endPosition, Easing.EaseInOut(_state, EasingType.Cubic, EasingType.Quartic));
            if (_state >= 1)
            {
                _moving = false;
                _state = 1;
            }
        }
    }

}
