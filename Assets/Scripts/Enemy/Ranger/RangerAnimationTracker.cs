using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAnimationTracker : MonoBehaviour {

    private EnemyBase _parent;
    private Ranger _ranger;

    void Awake()
    {
        _parent = GetComponentInParent<EnemyBase>();
        _ranger = (Ranger)_parent;
    }

    public void Shoot()
    {
        _ranger.Shoot();
    }

    public void AnimationReady()
    {
        _parent.SetState(EnemyBase.State.Move);
    }
}
