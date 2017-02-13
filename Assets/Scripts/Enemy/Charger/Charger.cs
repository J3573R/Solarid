using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charger : EnemyBase {

    void Awake()
    {
        SetState(EnemyBase.State.Idle);
    }    

    protected void Update()
    {

    }

}
