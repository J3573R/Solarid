using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBlink : AbilityBase {

    [SerializeField]
    private ParticleSystem _particle;

    private ParticleSystem _startParticle;
    private ParticleSystem _endParticle;
    private MeshRenderer[] _renderers;
    private SkinnedMeshRenderer _playerRender;
    private Vector3 _targetPosition;
    private Player _player;

	// Use this for initialization
	void Awake () {

        _startParticle = Instantiate(_particle, transform.position, Quaternion.identity);
        _endParticle = Instantiate(_particle, transform.position, Quaternion.identity);
        _startParticle.Stop();
        _endParticle.Stop();
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _playerRender = GetComponentInChildren<SkinnedMeshRenderer>();
        _player = GetComponent<Player>();
	}

    /// <summary>
    /// Performs the blink on the position of the mouse. 
    /// </summary>
    public override void Execute()
    {
        _targetPosition = _player.Input.GetMousePosition();

        if (_targetPosition != Vector3.zero)
        {
            _targetPosition.y = 0;
            _startParticle.transform.position = transform.position;

            foreach (MeshRenderer rend in _renderers)
            {
                rend.enabled = false;
            }
            _playerRender.enabled = false;
            transform.position = _targetPosition;
            _startParticle.Play();
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
        _endParticle.transform.position = _targetPosition;
        _endParticle.Play();
        foreach (MeshRenderer rend in _renderers)
        {
            rend.enabled = true;
        }
        
        _player.ShootingEnabled = true;
        _playerRender.enabled = true;
    }
}
