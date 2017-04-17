using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    
    public bool Shooting;
    public bool Casting;
    public float AimingRotationSpeed = 20f;
    public bool Initialized;
    public AudioClip AudioRun;

    private Player _player;
    private Rigidbody _rigidbody;
    private Vector2 _vMousePos;
    private Vector2 _fPlayerPosInScreen;
    private Vector2 _fDiff;
    private Vector3 _moveDirectionRay;
    private Vector3 _moveDirection;
    private float _moveSpeed = 5;
    private float _fSign;
    private float _fAngle;
    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _goingRight;
    private AudioSource _audio;

    // Use this for initialization
    void Start () {
        Init();
    }

    public void Init()
    {
        if (!Initialized)
        {
            _player = GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody>();
            _audio = GetComponent<AudioSource>();
            Initialized = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Shooting || Casting && !_player.AbilityController._allAbilitiesDisabled)
        {
            ListenMouse();
        }
    }

    /// <summary>
    /// Sets the player to shooting and changes the movement speed accordingly
    /// </summary>
    /// <param name="state"></param>
    public void SetShooting(bool state)
    {
        if (state)
        {            
            Shooting = state;
            _player.Animation.Casting = state;
            _moveSpeed = 3;

        }
        else if (!state && state != Shooting)
        {
            Shooting = false;
            _player.Animation.Casting = state;
            _moveSpeed = 5;
        }
    }

    /// <summary>
    /// Sets the player to Casting and changes the movement speed accordingly
    /// </summary>
    /// <param name="state"></param>
    public void SetCasting(bool state)
    {
        if (state)
        {
            Casting = state;
            _moveSpeed = 3;
            _player.AbilityController._currentCharge.Play();                
        }
        else if (!state)
        {
            Casting = false;
            _moveSpeed = 5;
            _player.AbilityController._currentCharge.Stop();  
        }
    }

    /// <summary>
    /// takes keyboard Input and increases horizontal & vertical velocity of player times Speed. Called from InputController
    /// </summary>
    public void Move(float inputX, float inputZ)
    {      
        if (inputX != 0 || inputZ != 0)
        {                        
            float rotationSpeed = 7;
            _player.Animation.Moving = true;
            _moveDirection = Vector3.zero;
            Vector3 previousLocation = transform.position;
            _moveDirection.x = inputX / 2;
            _moveDirection.z = inputZ / 2;

            if (!Shooting)
                _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.LookRotation(_moveDirection),
                    Time.fixedDeltaTime * rotationSpeed);

            _moveDirection = MovementBounds(_moveDirection);
            if (!_audio.isPlaying)
            {
                _audio.clip = AudioRun;
                _audio.Play();
            }

            _rigidbody.velocity = _moveDirection.normalized * _moveSpeed;
            CheckDirection(_moveDirection);
        }
        else
        {            
            _rigidbody.velocity = Vector3.zero;
            _player.Animation.Moving = false;
        }
    }

    /// <summary>
    /// Checks the direction player is moving towards, compares it to the directiuon he's facing and sets animation correctly
    /// </summary>
    /// <param name="direction">direction player moving towards</param>
    private void CheckDirection(Vector3 direction)
    {

        Vector3 forward = transform.forward;
        Vector3 left = transform.position + (transform.right * -1);
        Vector3 right = transform.position + transform.right * 1;

        _goingRight = CheckSide(transform.position + direction, left, right);
        
        float tmp = Vector3.Angle(forward, direction);

        if (tmp < 22.5f)
            _player.Animation.MoveDirection = PlayerAnimation.AnimationState.RunForward;
        if (tmp >= 157.5f)
            _player.Animation.MoveDirection = PlayerAnimation.AnimationState.RunBack;

        if (_goingRight)
        {
            //Debug.Log(direction.x);
            if (tmp >= 22.5f && tmp < 67.5f)
                _player.Animation.MoveDirection = PlayerAnimation.AnimationState.RunForwardRight;
            else if (tmp >= 67.5f && tmp < 112.5f)
                _player.Animation.MoveDirection = PlayerAnimation.AnimationState.RunRight;
            else if (tmp >= 112.5f && tmp < 157.5f)
                _player.Animation.MoveDirection = PlayerAnimation.AnimationState.RunBackRight;
        }
        else
        {
            if (tmp >= 22.5f && tmp < 67.5f)
                _player.Animation.MoveDirection = PlayerAnimation.AnimationState.RunForwardLeft;
            else if (tmp >= 67.5f && tmp < 112.5f)
                _player.Animation.MoveDirection = PlayerAnimation.AnimationState.RunLeft;
            else if (tmp >= 112.5f && tmp < 157.5f)
                _player.Animation.MoveDirection = PlayerAnimation.AnimationState.RunBackLeft;
        }                     
    }

    /// <summary>
    /// Checks which side the player is moving towards, left or right
    /// </summary>
    /// <param name="direction">Point in the moving direction</param>
    /// <param name="left">left side of player</param>
    /// <param name="right">right side of player</param>
    /// <returns>true if right, false if left</returns>
    private bool CheckSide(Vector3 direction, Vector3 left, Vector3 right)
    {
        float distanceLeft = Vector3.Distance(direction, left);
        float distanceRight = Vector3.Distance(direction, right);

        if (distanceRight > distanceLeft)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Checks if next step of movement goes over platform.
    /// </summary>
    /// <param name="horizontal">Horizontal movement direction</param>
    /// <param name="vertical">Vertical movement direction</param>
    /// <returns></returns>
    private bool CanMoveDirection(float horizontal, float vertical)
    {

        _moveDirectionRay = _rigidbody.transform.position;
        _moveDirectionRay.x += horizontal;
        _moveDirectionRay.z += vertical;
        _moveDirectionRay.y = 1;

        Ray ray = new Ray(_moveDirectionRay, Vector3.down);
        Debug.DrawRay(_moveDirectionRay, Vector3.down, Color.green, 0.1f);

        if (Physics.Raycast(ray, 2f))
        {
            Debug.Log(Physics.Raycast(ray, 2f));
            
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks horizontal and vertical directions and alters direction vector acording it.
    /// </summary>
    /// <param name="direction">Direction to check.</param>
    /// <returns>Modified direction.</returns>
    private Vector3 MovementBounds(Vector3 direction)
    {
        Vector3 result = new Vector3();

        _moveDirectionRay = _rigidbody.transform.position;
        _moveDirectionRay.x += direction.x;
        _moveDirectionRay.y = 1;

        Ray ray = new Ray(_moveDirectionRay, Vector3.down);

        if (Physics.Raycast(ray, 2f))
        {
            //Debug.Log(Physics.Raycast(ray, 2f));
            result.x = direction.x;            
        } else
        {
            _moveSpeed = 3;
        }

        _moveDirectionRay = _rigidbody.transform.position;
        _moveDirectionRay.z += direction.z;
        _moveDirectionRay.y = 1;
        ray = new Ray(_moveDirectionRay, Vector3.down);
        if (Physics.Raycast(ray, 2f))
        {
            //Debug.Log(Physics.Raycast(ray, 2f));
            result.z = direction.z;
        } else
        {
            _moveSpeed = 3;
        }

        result.y = 0;
        return result;
    }

    /// <summary>
    /// Defines mouse position from screen size and rotates the players Y axis by the angle.
    /// </summary>
    private void ListenMouse()
    {
        Vector3 tmp = _player.Input.GetMousePosition();
        tmp.y = transform.position.y;

        transform.LookAt(tmp);
        /*
        _vMousePos = Input.mousePosition;

        _fPlayerPosInScreen = Camera.main.WorldToScreenPoint(_player.transform.position);
        _fDiff = _fPlayerPosInScreen - _vMousePos;
        _fSign = (_fPlayerPosInScreen.y < _vMousePos.y) ? 1.0f : -1.0f;
        _fAngle = (Vector3.Angle(Vector3.right, _fDiff) * _fSign) - 90;

        transform.rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.Euler(0, _fAngle, 0), Time.fixedDeltaTime * AimingRotationSpeed);

    */
    }
}
