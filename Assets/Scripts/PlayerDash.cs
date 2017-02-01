using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour {

    private bool _targeting;
    private bool _moving;
    private Camera _camera;
    [SerializeField]
    private ParticleSystem _particleSystem;
    private MeshRenderer _renderer;
    private Vector3 _targetPosition;
    private Player _player;

	// Use this for initialization
	void Start () {
        _camera = FindObjectOfType<Camera>();

        _particleSystem = Instantiate(_particleSystem, transform.position, Quaternion.identity);
        _particleSystem.Stop();
        _renderer = GetComponent<MeshRenderer>();
        _player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_moving)
        {
            _particleSystem.transform.position = Vector3.MoveTowards(_particleSystem.transform.position, _targetPosition, Time.deltaTime * 20);
            transform.position = Vector3.MoveTowards(_particleSystem.transform.position, _targetPosition, Time.deltaTime * 20);

            if (_particleSystem.transform.position == _targetPosition)
            {
                _moving = false;
                //_renderer.enabled = true;
                _particleSystem.Stop();
                _player.ShootingEnabled = true;
            }
        }
		
	}

    public void Dash()
    {
        if (_targeting)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var heading = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1)) - _camera.transform.position;

            var distance = heading.magnitude;
            var direction = heading / distance; // This is now the normalized direction.

            Debug.Log(direction);

            Ray ray = new Ray(pos, direction);
            Debug.DrawRay(pos, direction, Color.red, 1);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 500f))
            {

                Debug.Log("OSUMA");

                _targetPosition = hit.point;
                Debug.Log(hit.transform.position);
                _targetPosition.y = 1;
                _particleSystem.transform.position = transform.position;
                //_renderer.enabled = false;              
                //transform.position = _targetPosition;
                
                
                _particleSystem.Play();
                _moving = true;

                _targeting = false;
            }
            
            
        }
    }

    public void Targeting()
    {
        if (_targeting)
        {
            Debug.Log("stoptargeting");
            _targeting = false;
            _player.ShootingEnabled = true;
        }
        else
        {
            _targeting = true;
            _player.ShootingEnabled = false;
        }         

    }
}
