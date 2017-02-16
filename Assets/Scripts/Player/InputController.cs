using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Rotation speed of the player
    public float RotationSpeed = 8f;
    public float AimingRotationSpeed = 20f;
    public PlayerAnimation PlayerAnimation;

    private bool _targeting;
    private Player _player;
    private Camera _camera;
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

    void Awake()
    {
        _player = GetComponent<Player>();
        _rigidbody = GetComponent<Rigidbody>();
        _camera = FindObjectOfType<Camera>();
        PlayerAnimation = FindObjectOfType<PlayerAnimation>(); 
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (!_targeting)
            {
                PlayerAnimation.SetAnimation(PlayerAnimation.AnimationState.Run);
            }
            
            Move();
        } else
        {
            PlayerAnimation.SetAnimation(PlayerAnimation.AnimationState.Idle);
            _rigidbody.velocity = Vector3.zero;
        }        
    }

    void Update()
    {
        GetInput();               

        if (_targeting) {            
            ListenMouse();
        }
            
    }

    /// <summary>
    /// Gets Input for various keys and calls methods accordingly
    /// </summary>
    private void GetInput()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Ability"))
        {
            PlayerAnimation.SetAnimation(PlayerAnimation.AnimationState.RunForward);
            _targeting = true;
            _moveSpeed = 3;
        }

        if (Input.GetButtonUp("Fire1") && !Input.GetButton("Ability"))
        {
            _targeting = false;
            _moveSpeed = 5;
        }        

        if (Input.GetButtonUp("Ability") && !Input.GetButton("Fire1"))
        {
            _player.AbilityController.Execute();
            _targeting = false;
            _moveSpeed = 5;
        }

        if (Input.GetButton("Fire1"))
        {
            _player.Shoot();
        }

        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("PRAISE THE SUN");
            PlayerAnimation.SetAnimation(PlayerAnimation.AnimationState.Praise);
        }
        
              
        if (Input.GetButtonUp("SetBlink"))        
            _player.AbilityController.SetAbility(AbilityController.Ability.Blink);
        if (Input.GetButtonUp("SetGrenade"))
            _player.AbilityController.SetAbility(AbilityController.Ability.Grenade);
        if (Input.GetButtonUp("SetThirdAbility"))
            _player.AbilityController.SetAbility(AbilityController.Ability.SomeRandomAbility);
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {            
            float tmp = Input.GetAxis("Mouse ScrollWheel");

            if (tmp < 0)
                _player.AbilityController.ScrollWeapon(-1);
            else if (tmp > 0)
                _player.AbilityController.ScrollWeapon(1);
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

        transform.rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.Euler(0, _fAngle, 0), Time.fixedDeltaTime * AimingRotationSpeed);    
    }

    /// <summary>
    /// Listens keyboard Input and increases horizontal & vertical velocity of player times Speed.
    /// </summary>
    private void Move()
    {
        
        float rotationSpeed = 7;

        _moveDirection = Vector3.zero;
        Vector3 previousLocation = transform.position;

        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.z = Input.GetAxisRaw("Vertical");

        if (CanMoveDirection(_moveDirection.x, _moveDirection.z))
        {            
            _rigidbody.velocity = _moveDirection.normalized * _moveSpeed;

            if (!_targeting)
                _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.LookRotation(_moveDirection),
                    Time.fixedDeltaTime*rotationSpeed);
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Checks if next step of movement goes over platform.
    /// </summary>
    /// <param name="horizontal">Horizontal movement direction</param>
    /// <param name="vertical">Vertical movement direction</param>
    /// <returns></returns>
    private bool CanMoveDirection(float horizontal, float vertical)
    {
        _moveDirectionRay = Globals.Player.transform.position;
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



