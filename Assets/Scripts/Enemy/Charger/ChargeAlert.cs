using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAlert : EnemyStateBase {

    public ChargeAlert(GameObject parent) : base(parent)
    {
        Parent = parent;
        eState = EnemyBase.State.Alert;
    }

    public override void Update()
    {
        Parent.transform.Rotate(0, 2, 0);
        Debug.Log("Alert");

    }
}
