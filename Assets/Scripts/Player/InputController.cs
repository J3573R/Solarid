using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Rotation speed of the player
    public float RotationSpeed = 8f;
    // Animation controller component in Player
    public PlayerAnimation PlayerAnimation;
    // Should the character face towards mouse?
    public bool ListenInput = true;
    public bool CinematicMovement { get; set; }
    public bool ShootingDisabled { get; set; }

    private Player _player;
    private Camera _camera;
    public bool Initialized;

    

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (!Initialized)
        {
            _player = GetComponent<Player>();
            _camera = FindObjectOfType<Camera>();
            //Globals.CameraScript.Init();
            PlayerAnimation = FindObjectOfType<PlayerAnimation>();
            Initialized = true;
        }        
    } 

    private void FixedUpdate()
    {
        if (!CinematicMovement)
        {
            if (ListenInput && !GameStateManager.Instance.GameLoop.Paused)
            {
                _player.Movement.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
            else
            {
                _player.Movement.Move(0, 0);
            }
        }
            
                 
    }

    void Update()
    {
        if (ListenInput && !GameStateManager.Instance.GameLoop.Paused)
        {
            GetMouseInput();
            GetKeyBoardInput();
        }        
    }    

    /// <summary>
    /// Gets Input from mouse and calls methods accordingly
    /// </summary>
    private void GetMouseInput()
    {
        //TODO: If this any larger, refactor whole shit. Or refactor anyways.

        if (Input.GetButton("Ability") && !_player.AbilityController.AllAbilitiesDisabled)
        {
            GameStateManager.Instance.GameLoop.References.CameraScript.AddMouseOffset(GetMousePosition());
        } else
        {
            GameStateManager.Instance.GameLoop.References.CameraScript.MouseOffset = Vector3.zero;
        }

        if (Input.GetButtonDown("Ability") && !_player.AbilityController.AllAbilitiesDisabled)
        {
            _player.Movement.SetCasting(true);
            _player.AbilityController.DrawRange(true);                        
        }   
        else if (!Input.GetButton("Ability"))
        {            
            _player.Movement.SetCasting(false);
            _player.AbilityController.DrawRange(false);
        }
        
        if (Input.GetButtonUp("Ability") && !Input.GetButton("Fire1"))
        {            
            _player.Movement.SetShooting(false);
            _player.Movement.SetCasting(false);
        }

        if (Input.GetButton("Fire1"))
        {                                           
            if (!_player.Movement.Casting && !ShootingDisabled)
            {
                GameStateManager.Instance.GameLoop.References.CameraScript.AddMouseOffset(GetMousePosition());
                _player.Movement.SetShooting(true);                
                _player.Shoot();
            }    
        } else
        {
            if (!_player.Movement.Casting)
                _player.Movement.SetShooting(false);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            _player.Movement.SetShooting(false);
            _player.Gun.SetShooting(false);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _player.AbilityController.Execute();

            if (!_player.Movement.Casting && !ShootingDisabled)
            {
                _player.Gun.SetShooting(true);
                _player.Movement.SetShooting(true);
            }
        }        

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
    /// Gets Input from keyboard and does stuff accordingly
    /// </summary>
    private void GetKeyBoardInput()
    {
        if (Input.GetButtonDown("Reload"))
        {
            _player.Gun.InitiateReload();
        }

        if (Input.GetButtonDown("Interact"))
        {
            _player.Interact = true;
        }

        if (Input.GetButtonUp("Interact"))
        {
            _player.Interact = false;
        }

        if (Input.GetKey(KeyCode.P))
        {
            PlayerAnimation.SetAnimation(PlayerAnimation.AnimationState.Praise);
        }

        if (Input.GetButtonUp("SetBlink"))
            _player.AbilityController.SetAbility(AbilityController.Ability.Blink);
        if (Input.GetButtonUp("SetGrenade"))
            _player.AbilityController.SetAbility(AbilityController.Ability.Vortex);
        if (Input.GetButtonUp("SetConfusion"))
            _player.AbilityController.SetAbility(AbilityController.Ability.Clone);
    }

    /// <summary>
    /// uses raycast to determine mouseposition and returns it. Works in default layer
    /// </summary>
    /// <returns>Point of the mouse in world space. If ray didn't hit, return Vector3.zero</returns>
    public Vector3 GetMouseGroundPosition()
    {
        var layerMask = 1 << 8;
        layerMask = ~layerMask;
        Vector3 pos = _camera.transform.position;
        var heading = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) - _camera.transform.position;

        var distance = heading.magnitude;
        var direction = heading / distance; 

        Ray ray = new Ray(pos, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            if (hit.transform.tag.Equals("Ground"))
                return hit.point;
        }
             
        return Vector3.zero;
    }

    /// <summary>
    /// uses raycast to determine mouseposition and returns it. 
    /// Works in MouseTracker layer so the mouse position is allways accessible and in certaing height
    /// </summary>
    /// <returns>position</returns>
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
