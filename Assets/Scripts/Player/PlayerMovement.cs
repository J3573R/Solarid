﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Player _player;
    private Rigidbody _rigidbody;
    public bool Targeting;
    public float AimingRotationSpeed = 20f;
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


    // Use this for initialization
    void Start () {
        _player = GetComponent<Player>();
        _rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Targeting)
        {
            ListenMouse();
        }
    }

    public void SetTargeting(bool state)
    {
        if (state)
        {
            Targeting = state;
            _player.Animation.Targeting = state;
            _moveSpeed = 3;

        }
        else
        {
            Targeting = false;
            _player.Animation.Targeting = state;
            _moveSpeed = 5;
        }
    }

    private void FixedUpdate()
    {
        
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

            _moveDirection.x = inputX;
            _moveDirection.z = inputZ;

            CheckDirection(_moveDirection);

            if (CanMoveDirection(_moveDirection.x, _moveDirection.z))
            {
                _rigidbody.velocity = _moveDirection.normalized * _moveSpeed;

                if (!Targeting)
                    _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.LookRotation(_moveDirection),
                        Time.fixedDeltaTime * rotationSpeed);
            }
            else
            {
                _rigidbody.velocity = Vector3.zero;
                
            }
        }
        else
        {
            
            _rigidbody.velocity = Vector3.zero;
            _player.Animation.Moving = false;
        }
    }

    private void CheckDirection(Vector3 direction)
    {
        Vector3 forward = transform.forward;
        
        float tmp = Vector3.Angle(forward, direction);

        //Debug.Log(Vector3.Angle(forward, direction));
        if (tmp < 15)
            _player.Animation.MoveDirection = PlayerAnimation.AnimationState.RunForward;
        if (tmp > 165)
            _player.Animation.MoveDirection = PlayerAnimation.AnimationState.RunBack;
        
        //if (tmp >= )
        
        
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
            //Debug.Log(Physics.Raycast(ray, 2f));
            
            return true;
        }
        Debug.Log("TOIMII");
        return false;
    }

    /// <summary>
    /// Defines mouse position from screen size and rotates the players Y axis by the angle.
    /// </summary>
    private void ListenMouse()
    {
        _vMousePos = Input.mousePosition;

        _fPlayerPosInScreen = Camera.main.WorldToScreenPoint(_player.transform.position);
        _fDiff = _fPlayerPosInScreen - _vMousePos;
        _fSign = (_fPlayerPosInScreen.y < _vMousePos.y) ? 1.0f : -1.0f;
        _fAngle = (Vector3.Angle(Vector3.right, _fDiff) * _fSign) - 90;

        transform.rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.Euler(0, _fAngle, 0), Time.fixedDeltaTime * AimingRotationSpeed);
    }
}