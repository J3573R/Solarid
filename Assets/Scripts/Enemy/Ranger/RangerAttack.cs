using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttack : EnemyStateBase
{
    private Ranger _ranger;
    private float _animationDuration = 0;

    protected override void Start()
    {
        base.Start();
        _ranger = (Ranger) Parent;
    }

    protected override void Update()
    {
        if (_ranger.ReadyToShoot <= 0)
        {
            Parent.Animator.SetInteger("animState", (int)EnemyBase.AnimationState.Attack);
            if (Parent.Animator.GetCurrentAnimatorStateInfo(0).length - 0.35f < _animationDuration)
            {
                GameObject bullet = _ranger.RangerBulletPool.GetBullet();
                bullet.transform.position = gameObject.transform.position;
                bullet.transform.rotation = gameObject.transform.rotation;
                EnemyBullet b = bullet.GetComponent<EnemyBullet>();
                b.MyPool = _ranger.RangerBulletPool;
                b.Damage = Parent.Damage;
                _animationDuration = 0;
                _ranger.ReadyToShoot = 2f;
            }
            _animationDuration += Time.deltaTime;
        }
        else if(_ranger.ReadyToShoot > 0)
        {
            Parent.SetState(EnemyBase.State.Move);   
        }
    }
}
