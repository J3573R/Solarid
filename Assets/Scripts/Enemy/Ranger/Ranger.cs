using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RangerBulletPool))]
public class Ranger : EnemyBase
{
    public float ReadyToShoot;
    public RangerBulletPool RangerBulletPool;


    private Vector3 _positionAtLastFrame;
    private float _distanceAtLastFrame;

    protected override void Init()
    {
        base.Init();
        SetState(EnemyBase.State.Idle);
        RangerBulletPool = GetComponent<RangerBulletPool>();
        StartPosition = transform.position;
        Initialized = true;
    }

    protected override void Update()
    {
        base.Update();

        if (Animator != null && CurrentState != State.Attack)
        {
            if (_positionAtLastFrame == transform.position)
            {
                Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Idle);
            }
            else
            {
                if(CurrentState != State.Move)
                {
                    Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Walk);
                }
                else
                {
                    float distance = Vector3.Distance(transform.position, Target.transform.position);
                    if (distance < _distanceAtLastFrame)
                    {
                        Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Walk);
                    }
                    else if (distance > _distanceAtLastFrame)
                    {
                        Animator.SetInteger("animState", (int)EnemyBase.AnimationState.WalkBack);
                    }
                    _distanceAtLastFrame = distance;
                }             
                
            }

            _positionAtLastFrame = transform.position;
        }

        if (ReadyToShoot > 0)
        {
            ReadyToShoot = ReadyToShoot - Time.deltaTime;
        }
    }

    public override void SetState(EnemyBase.State state)
    {
        if (CurrentState != state)
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
                    AlertOthers();
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

    public void Shoot()
    {
        GameObject bullet = RangerBulletPool.GetBullet();
        bullet.transform.position = gameObject.transform.position;
        bullet.transform.rotation = gameObject.transform.rotation;
        EnemyBullet b = bullet.GetComponent<EnemyBullet>();
        b.MyPool = RangerBulletPool;
        b.Damage = Damage;
        ReadyToShoot = 2f;
    }
}
