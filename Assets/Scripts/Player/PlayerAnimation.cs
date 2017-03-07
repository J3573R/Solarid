using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public Animator Animator;
    private Player _player;
    public bool Casting;
    public bool Moving;
    public bool CastOnce;
    public AnimationState MoveDirection;
    public float CastTime;

    private AnimationState _currentState;
    private AnimationStance _currentStance;

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
        Cast = 10,
        Praise = 30
    }

    public enum AnimationStance
    {
        Idle = 0,
        Aiming = 1,
        Casting = 2
    }

	void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        _player = GetComponent<Player>();  
    }

    private void Update()
    {
        CheckAnimationStance();
        CheckAnimation();
    }

    private void CheckAnimation()
    {
        if (_currentStance == AnimationStance.Aiming)
        {
            if (!Moving)
                SetAnimation(AnimationState.Idle);
            else if (Moving && MoveDirection == AnimationState.RunForward)
                SetAnimation(AnimationState.RunForward);
            else if (Moving && MoveDirection == AnimationState.RunForwardRight)
                SetAnimation(AnimationState.RunForwardRight);
            else if (Moving && MoveDirection == AnimationState.RunRight)
                SetAnimation(AnimationState.RunRight);
            else if (Moving && MoveDirection == AnimationState.RunBackRight)
                SetAnimation(AnimationState.RunBackRight);
            else if (Moving && MoveDirection == AnimationState.RunBack)
                SetAnimation(AnimationState.RunBack);
            else if (Moving && MoveDirection == AnimationState.RunBackLeft)
                SetAnimation(AnimationState.RunBackLeft);
            else if (Moving && MoveDirection == AnimationState.RunLeft)
                SetAnimation(AnimationState.RunLeft);
            else if (Moving && MoveDirection == AnimationState.RunForwardLeft)
                SetAnimation(AnimationState.RunForwardLeft);
            
        }
        
        
                 
    
        if (_currentStance == AnimationStance.Casting)
        {
            if (CastOnce)
            {
                SetAnimation(AnimationState.Cast);
                CastOnce = false;
                StartCoroutine(ResetCastIdle());
            }


        }
        else if (_currentStance == AnimationStance.Idle)
        {
            if (Moving)
                SetAnimation(AnimationState.Run);
            else
                SetAnimation(AnimationState.Idle);                           
        }
    }

    private void CheckAnimationStance()
    {
        if (_player.Movement.Casting)        
            SetAnimationStance(AnimationStance.Casting);        
        else if (_player.Movement.Shooting)
            SetAnimationStance(AnimationStance.Aiming);
        else
            SetAnimationStance(AnimationStance.Idle);
    }

    private IEnumerator ResetCastIdle()
    {
        yield return new WaitForSeconds(CastTime);
        SetAnimation(AnimationState.Idle);

        
    }

    private void SetAnimationStance(AnimationStance stance)
    {
        if (stance != _currentStance)
        {
            StopAllCoroutines();
            _currentStance = stance;
            Animator.SetInteger("stance", (int)stance);
        }
    }

    public void SetAnimation(AnimationState animation)
    {
        if(animation != _currentState)
        {
            //Debug.Log("ANIMATION : " + animation);
            _currentState = animation;
            Animator.SetInteger("animState", (int)animation);
        }        
    }
}
