using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerIdle : EnemyStateBase {

    public ChargerIdle(GameObject parent) : base(parent)
    {
        Parent = parent;
        eState = EnemyBase.State.Idle;
    }

    public override void Update()
    {
        Parent.transform.Rotate(0, 1, 0);
        Debug.Log("Idling");
    }
}
