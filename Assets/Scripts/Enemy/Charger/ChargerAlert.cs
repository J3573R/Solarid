using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAlert : EnemyStateBase {

    float _timer = 0;

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Alert;
    }

    protected override void Update()
    {
        transform.LookAt(Globals.Player.transform);

        if(_timer >= 1)
        {
            Parent.SetState(EnemyBase.State.Move);
        }

        _timer += Time.deltaTime;
    }
}
