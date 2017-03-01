using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttack : EnemyStateBase
{
    private Ranger _ranger;

    protected override void Awake()
    {
        base.Awake();
        _ranger = (Ranger) Parent;
    }

    protected override void Update()
    {
        if (_ranger.ReadyToShoot <= 0)
        {
            GameObject bullet = _ranger.RangerBulletPool.GetBullet();
            bullet.transform.position = gameObject.transform.position;
            bullet.transform.rotation = gameObject.transform.rotation;
            EnemyBullet b = bullet.GetComponent<EnemyBullet>();
            b.MyPool = _ranger.RangerBulletPool;
            b.Damage = Parent.Damage;
            _ranger.ReadyToShoot = 2f;
        }
        else
        {
            Parent.SetState(EnemyBase.State.Move);   
        }
    }
}
