using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerIdle : EnemyStateBase {

    protected override void Awake()
    {
        base.Awake();
        eState = EnemyBase.State.Idle;
    }

    protected override void Update()
    {
        Parent.transform.Rotate(0, 1, 0);

        RaycastHit hit;

        if (Physics.Linecast(transform.position, Globals.Player.transform.position, out hit))
        {
            if (hit.distance < 8)
            {
                Parent.SetState(EnemyBase.State.Alert);
            }
        }
    }
}
