using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAlert : EnemyStateBase {

    float _timer = 0;
    private float _rotationSpeed = 2;
    private float _step;
    private Vector3 _targetDirection;
    private Vector3 _newDirection;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Alert;
    }

    protected override void Update()
    {
        _targetDirection = Globals.Player.transform.position - transform.position;
        _step = _rotationSpeed * Time.deltaTime;
        _newDirection = Vector3.RotateTowards(transform.forward, _targetDirection, _step, 0.0F);
        transform.rotation = Quaternion.LookRotation(_newDirection);

        _targetDirection = Globals.Player.transform.position - transform.position;


        if (_timer >= 1)
        {
            Parent.SetState(EnemyBase.State.Move);
        }

        _timer += Time.deltaTime;
    }
}
