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

    private AnimationState _currentState;
    private AnimationStance _currentStance;

    public enum AnimationState
    {
        Idle = 0,
        Run = 1,
        CastingCharge = 2,
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

        }
        /*
        if (Moving && !_player.Movement.Casting && !_player.Movement.Shooting)
            SetAnimation(AnimationState.Run);
        else if (!_player.Movement.Casting)
            SetAnimation(AnimationState.RunForward);
        else if (Moving && Targeting && MoveDirection == AnimationState.RunForward)
            SetAnimation(AnimationState.RunForward);
        else if (Moving && Targeting && MoveDirection == AnimationState.RunBack)
            SetAnimation(AnimationState.RunBack);
        else
                 
    */
        if (_currentStance == AnimationStance.Casting)
        {
            if (CastOnce)
            {
                SetAnimation(AnimationState.Cast);
                CastOnce = false;
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

    private void SetAnimationStance(AnimationStance stance)
    {
        if (stance != _currentStance)
        {
            Debug.Log("SetStance: " + stance);
            _currentStance = stance;
            Animator.SetInteger("stance", (int)stance);
        }
    }

    public void SetAnimation(AnimationState animation)
    {
        if(animation != _currentState)
        {
            Debug.Log("SetAnimation" + animation);
            _currentState = animation;
            Animator.SetInteger("animState", (int)animation);
        }        
    }
}
