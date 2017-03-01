﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RangerBulletPool))]
public class Ranger : EnemyBase
{

    public float ReadyToShoot;
    public RangerBulletPool RangerBulletPool;

    protected override void Awake()
    {
        base.Awake();
        SetState(EnemyBase.State.Idle);
        RangerBulletPool = GetComponent<RangerBulletPool>();
    }

    protected override void Update()
    {
        base.Update();
        if (ReadyToShoot > 0)
        {
            ReadyToShoot = Mathf.Clamp(ReadyToShoot - Time.deltaTime, 0, Mathf.Infinity);
        }
    }

    public override void SetState(EnemyBase.State state)
    {
        if (CurrentStateObject != null)
        {
            Destroy(CurrentStateObject);
        }

        switch (state)
        {
            case State.Idle:
                CurrentStateObject = gameObject.AddComponent<RangerIdle>();
                CurrentState = state;
                break;
            case State.Alert:
                CurrentStateObject = gameObject.AddComponent<RangerAlert>();
                CurrentState = state;
                break;
            case State.Move:
                CurrentStateObject = gameObject.AddComponent<RangerMove>();
                CurrentState = state;
                break;
            case State.Attack:
                CurrentStateObject = gameObject.AddComponent<RangerAttack>();
                CurrentState = state;
                break;
        }
    }
}
