using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charger : EnemyBase
{
    // Changes enemy state to alert when distance is smaller than this
    public int AlertDistance = 5;

    // Changes enemy state to idle when distance is bigger than this
    public int DisengageDistance = 7;

    protected override void Awake()
    {
        base.Awake();
        SetState(EnemyBase.State.Idle);
    }
}
