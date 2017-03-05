using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Rotation speed of the player
    public float RotationSpeed = 8f;

    public PlayerAnimation PlayerAnimation;
    
    private Player _player;
    private Camera _camera;   

    void Awake()
    {
        _player = GetComponent<Player>();        
        _camera = FindObjectOfType<Camera>();
        PlayerAnimation = FindObjectOfType<PlayerAnimation>(); 
    }

    private void FixedUpdate()
    {
        _player.Movement.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));            
    }

    void Update()
    {
        GetInput();
    }

    /// <summary>
    /// Gets Input for various keys and calls methods accordingly
    /// </summary>
    private void GetInput()
    {
        if (Input.GetButtonDown("Ability"))
        {
            _player.Movement.SetCasting(true);
        }         
        else if (Input.GetButtonUp("Ability"))
        {
            _player.Movement.SetCasting(false);
        }
        
        if (Input.GetButtonUp("Ability") && !Input.GetButton("Fire1"))
        {
            _player.Movement.SetShooting(false);
            _player.Movement.SetCasting(false);
        }

        if (Input.GetButton("Fire1"))
        {                                           
            if (!_player.Movement.Casting)
            {
                _player.Movement.SetShooting(true);                
                _player.Shoot();
            } else
            {
                _player.AbilityController.Execute();
            }            
        } else
        {
            _player.Movement.SetShooting(false);
            _player.Gun.SetShooting(false);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _player.AbilityController.Execute();
            _player.Gun.SetShooting(true);
        }

        if (Input.GetButtonDown("Interact"))
        {
            Globals.Interact = true;
        }

        if (Input.GetButtonUp("Interact"))
        {
            Globals.Interact = false;
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
    /// uses raycast to determine mouseposition and returns it. Works in default layer
    /// </summary>
    /// <returns>Point of the mouse in world space. If ray didn't hit, return Vector3.zero</returns>
    public Vector3 GetMouseGroundPosition()
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

    public Vector3 GetMousePosition()
    {
        var layerMask = 1 << 8;
        Vector3 pos = _camera.transform.position;
        var heading = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) - _camera.transform.position;

        var distance = heading.magnitude;
        var direction = heading / distance;

        Ray ray = new Ray(pos, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
    }    
}
