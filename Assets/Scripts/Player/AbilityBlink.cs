using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBlink : AbilityBase
{

    public AudioClip AudioBlink;

    [SerializeField]
    private ParticleSystem _particle;
    [SerializeField]
    private float _castDelayInSeconds;

    private ParticleSystem _startParticle;
    private ParticleSystem _endParticle;
    private MeshRenderer[] _renderers;
    private SkinnedMeshRenderer _playerRender;
    private Vector3 _targetPosition;
    private Player _player;
    private AudioSource _audio;

	// Use this for initialization
	void Awake () {

        _startParticle = Instantiate(_particle, transform.position, Quaternion.identity);
        _endParticle = Instantiate(_particle, transform.position, Quaternion.identity);
        _startParticle.Stop();
        _endParticle.Stop();
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _playerRender = GetComponentInChildren<SkinnedMeshRenderer>();
        _player = GetComponent<Player>();
	    _audio = GetComponent<AudioSource>();
	}

    /// <summary>
    /// Performs the blink on the position of the mouse. 
    /// </summary>
    public override void Execute(Vector3 targetPos)
    {
        _targetPosition = targetPos;
        _player.Animation.CastOnce = true;
        StartCoroutine(CastDelay());
        _targetPosition.y = 0;
        _startParticle.transform.position = transform.position;
        _audio.clip = AudioBlink;
        _audio.Play();
        _player.Health.Invulnerable = true;
    }

    /// <summary>
    /// Delay before the actual execution so animation can complete
    /// </summary>
    /// <returns></returns>
    private IEnumerator CastDelay()
    {
        yield return new WaitForSeconds(_castDelayInSeconds);
        foreach (MeshRenderer rend in _renderers)
        {
            rend.enabled = false;
        }
        _playerRender.enabled = false;
        transform.position = _targetPosition;
        _startParticle.Play();
        StartCoroutine(BlinkDelay());
        _player.Input.ListenInput = false;
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
        _player.Input.ListenInput = true;
        _player.Health.Invulnerable = false;
    }
}
