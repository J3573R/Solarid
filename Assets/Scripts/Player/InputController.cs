using UnityEngine;

public class InputController : MonoBehaviour
{
    // Speed of the player
    public float Speed = 5f;
    // Rotation speed of the player
    public float RotationSpeed = 8f;

    private Player _player;
    private Camera _camera;
    private Rigidbody _rigidbody;
    private Vector2 _vMousePos;
    private Vector2 _fPlayerPosInScreen;
    private Vector2 _fDiff;
    private Vector3 _moveDirectionRay;
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
        
        _fPlayerPosInScreen = Camera.main.WorldToScreenPoint(_player.transform.position);
        _fDiff = _fPlayerPosInScreen - _vMousePos;
        _fSign = (_fPlayerPosInScreen.y < _vMousePos.y) ? 1.0f : -1.0f;
        _fAngle = (Vector3.Angle(Vector3.right, _fDiff) * _fSign) - 90;

        transform.rotation = Quaternion.Euler(0, _fAngle, 0);

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
        _fHDir = Input.GetAxisRaw("Horizontal");
        _fVDir = Input.GetAxisRaw("Vertical");
        if (CanMoveDirection(_fHDir, _fVDir))
        {
            _rigidbody.velocity = new Vector3(_fHDir*Speed, 0f, _fVDir*Speed);
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
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var heading = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1)) - _camera.transform.position;

        var distance = heading.magnitude;
        var direction = heading / distance; 

        Ray ray = new Ray(pos, direction);
        Debug.DrawRay(pos, direction, Color.red, 1);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            return hit.point;
        }
             
        return Vector3.zero;
    }
    
}



