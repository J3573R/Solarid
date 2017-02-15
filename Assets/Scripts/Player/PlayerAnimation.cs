using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public Animator Animator;

    private AnimationState _currentState;

    public enum AnimationState
    {
        Idle = 0,
        Run = 1,
        RunForward = 2,
        RunForwardRight = 3,
        RunRight = 4,
        RunBackRight = 5,
        RunBack = 6,
        RunBackLeft = 7,
        RunLeft = 8,
        RunForwardLeft = 9,
        Praise = 30
    }

	void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
    }

    public void SetAnimation(AnimationState animation)
    {
        if(animation != _currentState)
        {
            _currentState = animation;
            Animator.SetInteger("animState", (int)animation);
        }        
    }
}
