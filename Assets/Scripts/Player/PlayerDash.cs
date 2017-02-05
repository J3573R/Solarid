using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : AbilityBase {

    private bool _targeting;
    private bool _moving;
    private Camera _camera;
    [SerializeField]
    private ParticleSystem _particleSystem;
    private MeshRenderer[] _renderers;
    private Vector3 _targetPosition;
    private Player _player;

	// Use this for initialization
	void Awake () {
        _camera = FindObjectOfType<Camera>();

        _particleSystem = Instantiate(_particleSystem, transform.position, Quaternion.identity);
        _particleSystem.Stop();
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _player = GetComponent<Player>();
	}

    /// <summary>
    /// Performs the blink on the position of the mouse. 
    /// </summary>
    public override void Execute()
    {
        _targetPosition = _player.input.GetMousePosition();

        if (_targetPosition != Vector3.zero)
        {
            _targetPosition.y = 1;
            _particleSystem.transform.position = transform.position;

            foreach (MeshRenderer rend in _renderers)
            {
                rend.enabled = false;
            }
            transform.position = _targetPosition;
            _particleSystem.Play();
            StartCoroutine(BlinkDelay());
        }               
    }

    /// <summary>
    /// Sets the delay for the blink.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BlinkDelay()
    {        
        yield return new WaitForSeconds(0.5f);        
        _particleSystem.transform.position = _targetPosition;
        _particleSystem.Play();
        foreach (MeshRenderer rend in _renderers)
        {
            rend.enabled = true;
        }
        
        _player.ShootingEnabled = true;
    }
}
