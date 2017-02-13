using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Speed of the player
    public float Speed = 5f;
    // Rotation speed of the player
    public float RotationSpeed = 8f;

    private bool _targeting;
    private Player _player;
    private Camera _camera;
    private Rigidbody _rigidbody;
    private Vector2 _vMousePos;
    private Vector2 _fPlayerPosInScreen;
    private Vector2 _fDiff;
    private Vector3 _moveDirectionRay;
    private float _moveSpeed = 5;
    private float _fSign;
    private float _fAngle;
    private float _fHDir;
    private float _fVDir;

    void Awake()
    {
        _player = GetComponent<Player>();
        _rigidbody = GetComponent<Rigidbody>();
        _camera = FindObjectOfType<Camera>();
    }


    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            Move();
        }        
    }

    void Update()
    {


        if (Input.GetButton("Fire1"))
        {
            _player.Shoot();            
        }
        if (Input.GetButtonDown("Fire1"))
        {
            _targeting = true;
            _moveSpeed = 3;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            _targeting = false;
            _moveSpeed = 5;
        }
        Debug.Log(_moveSpeed);
        
        GetInput();

        if (_targeting)
            ListenMouse();
    }

    /// <summary>
    /// Gets input for various keys and calls methods accordingly
    /// </summary>
    private void GetInput()
    {
        if (Input.GetButtonDown("Ability"))
        {
            _player.abilityController.Target();
            _targeting = true;
            _moveSpeed = 3;
        }
        if (Input.GetButtonUp("Ability"))
        {
            _player.abilityController.Execute();
            _targeting = false;
            _moveSpeed = 5;
        }
        
              
        if (Input.GetButtonUp("SetBlink"))        
            _player.abilityController.SetAbility(AbilityController.Ability.Blink);
        if (Input.GetButtonUp("SetGrenade"))
            _player.abilityController.SetAbility(AbilityController.Ability.Grenade);
        if (Input.GetButtonUp("SetThirdAbility"))
            _player.abilityController.SetAbility(AbilityController.Ability.SomeRandomAbility);
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {            
            float tmp = Input.GetAxis("Mouse ScrollWheel");

            if (tmp < 0)
                _player.abilityController.ScrollWeapon(-1);
            else if (tmp > 0)
                _player.abilityController.ScrollWeapon(1);
        }

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

        transform.rotation = Quaternion.Euler(0, _fAngle, 0);
        
               
    }

    /// <summary>
    /// Listens keyboard input and increases horizontal & vertical velocity of player times Speed.
    /// </summary>
    private void Move()
    {
        
        float rotationSpeed = 7;

        Vector3 moveDirection = Vector3.zero;
        Vector3 previousLocation = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection.z = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection.z = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection.x = 1;
        }

        if (CanMoveDirection(moveDirection.x, moveDirection.z))
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + moveDirection.normalized, Time.fixedDeltaTime * _moveSpeed);

            if (!_targeting)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - previousLocation), Time.fixedDeltaTime * rotationSpeed);
        }

        

        /*
        _fHDir = Input.GetAxis("Horizontal");
        _fVDir = Input.GetAxis("Vertical");
        _rigidbody.velocity = new Vector3(_fHDir * Speed, 0f, _fVDir * Speed);
        */
    }
        

    

    /// <summary>
    /// Checks if next step of movement goes over platform.
    /// </summary>
    /// <param name="horizontal">Horizontal movement direction</param>
    /// <param name="vertical">Vertical movement direction</param>
    /// <returns></returns>
    private bool CanMoveDirection(float horizontal, float vertical)
    {
        _moveDirectionRay = _player.transform.position;
        _moveDirectionRay.x += horizontal;
        _moveDirectionRay.z += vertical;

        Ray ray = new Ray(_moveDirectionRay, Vector3.down);
        //Debug.DrawRay(_moveDirectionRay, Vector3.down, Color.green, 0.1f);

        if (Physics.Raycast(ray, 2f))
        {
            return true;
        }
        
        return false;
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



