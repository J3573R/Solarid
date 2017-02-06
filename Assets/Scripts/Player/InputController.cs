﻿using UnityEngine;

public class InputController : MonoBehaviour
{
    // Speed of the player
    public float Speed = 5f;
    // Rotation speed of the player
    public float RotationSpeed = 8f;

    private Player _player;
    private Camera _camera;
    private Rigidbody _rigidbody;
    private Vector3 _vMousePos;
    private float _fAngle;
    private float _fHDir;
    private float _fVDir;

    void Awake()
    {
        _player = GetComponent<Player>();
        _rigidbody = GetComponent<Rigidbody>();
        _camera = FindObjectOfType<Camera>();
    }

    void Update()
    {        
        Move();

        if (Input.GetButton("Fire1"))
        {
            _player.Shoot();
        }

        ListenMouse();
    }

    /// <summary>
    /// Defines mouse position from screen size and rotates the players Y axis by the angle.
    /// </summary>
    private void ListenMouse()
    {
        _vMousePos = Input.mousePosition;

        _vMousePos.x -= (float)Screen.width / 2;
        _vMousePos.y -= (float)Screen.height / 2;

        _vMousePos += transform.position;

        _fAngle = Vector3.Angle(_vMousePos, Vector3.up);

        if (_vMousePos.x < 0)
            _fAngle = 360 - _fAngle;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _fAngle, 0), Time.deltaTime * RotationSpeed);

        if (Input.GetButtonDown("Ability"))
        {            
            _player.abilityController.Execute();
        }           
    }

    /// <summary>
    /// Listens keyboard input and increases horizontal & vertical velocity of player times Speed.
    /// </summary>
    private void Move()
    {
        _fHDir = Input.GetAxis("Horizontal");
        _fVDir = Input.GetAxis("Vertical");
        _rigidbody.velocity = new Vector3(_fHDir * Speed, 0f, _fVDir * Speed);
    }

    /// <summary>
    /// uses raycast to determine mouseposition and returns it.
    /// </summary>
    /// <returns>Point of the mouse in world space. If ray didn't hit, return Vector3.zero</returns>
    public Vector3 GetMousePosition()
    {
        //Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos = _camera.transform.position;
        var heading = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) - _camera.transform.position;

        var distance = heading.magnitude;
        var direction = heading / distance; 

        Ray ray = new Ray(pos, direction);
        Debug.DrawRay(pos, direction * 20, Color.green, 5, false);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            return hit.point;
        }
             
        return Vector3.zero;
    }
}



