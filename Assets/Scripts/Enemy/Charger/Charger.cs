using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charger : EnemyBase
{

    public int AlertDistance = 5;
    public int DisengageDistance = 7;

    protected override void Awake()
    {
        base.Awake();
        SetState(EnemyBase.State.Idle);
    }    

    protected void Update()
    {

    }

}
