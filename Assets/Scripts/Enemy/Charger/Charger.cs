using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : EnemyBase {

    void Awake()
    {
        SetState(EnemyBase.State.Idle);
    }

    protected override void Update()
    {
        base.Update();
        RaycastHit hit;

        if (Physics.Linecast(transform.position, Globals.Player.transform.position, out hit))
        {
            if (hit.distance > 10)
            {
                SetState(EnemyBase.State.Idle);
            }
            else
            {
                SetState(EnemyBase.State.Alert);
            }
            
        }

        //Debug.Log(CurrentState);
    }

    public void SetState(EnemyBase.State state)
    {
        switch (state)
        {
            case State.Idle:
                CurrentState = new ChargerIdle(gameObject);
                break;
            case State.Alert:
                CurrentState = new ChargeAlert(gameObject);
                break;
        }
    }

}
