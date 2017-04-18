using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVortex : AbilityBase
{
    private Player _player;
    private Vector3 _target;

    [SerializeField] private GameObject _blast;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    /// <summary>
    /// Gets the mouseposition and throws the grenade to it
    /// </summary>
    public override void Execute(Vector3 targetPos)
    {
        _target = targetPos;

        _player.Animation.CastOnce = true;
        StartCoroutine(CastDelay());
        

    }

    /// <summary>
    /// Delay before the actual execution so animation can complete
    /// </summary>
    /// <returns></returns>
    private IEnumerator CastDelay()
    {
        yield return new WaitForSeconds(_player.AbilityController.CastDelayInSeconds);
        if (_target != Vector3.zero)
        {
            _target.y = 1;
            Instantiate(_blast, _target, Quaternion.Euler(90, 0, 0));
        }
    }

}
