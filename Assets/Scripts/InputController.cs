using UnityEngine;

public class InputController : MonoBehaviour
{
    // Speed of the player
    public float Speed = 5f;
    // Rotation speed of the player
    public float RotationSpeed = 8f;

    private Player _player;
    private Rigidbody _rigidbody;
    private Vector3 _vMousePos;
    private float _fAngle;
    private float _fHDir;
    private float _fVDir;

    void Awake()
    {
        _player = FindObjectOfType<Player>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ListenMouse();
        Move();
        if (Input.GetButtonDown("Fire1"))
        {
            _player.Shoot();
        }
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

}



