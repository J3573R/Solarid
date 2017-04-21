using System;
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
    private bool _stillCasting;
    public bool Initialized;

    /// <summary>
    /// Current animation state, within current stance
    /// </summary>
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

    /// <summary>
    /// Top level of the animation hierarchy. Which of the 3 Animation statemachines are we gonna use
    /// </summary>
    public enum AnimationStance
    {
        Idle = 0,
        Aiming = 1,
        Casting = 2
    }

	void Awake()
    {
        Init(); 
    }

    public void Init()
    {
        if (!Initialized)
        {
            Animator = GetComponentInChildren<Animator>();
            _player = GetComponent<Player>();
            Initialized = true;
        }
    }

    private void Update()
    {
        if (!GameStateManager.Instance.GameLoop.Paused || _player.Input.CinematicMovement)
        {
            if (!_stillCasting)
            {
                CheckAnimationStance();

                if (_currentStance == AnimationStance.Aiming)
                    CheckAimAnimation();
                else if (_currentStance == AnimationStance.Casting)
                    CheckCastAnimation();
                else if (_currentStance == AnimationStance.Idle)
                    CheckIdleAnimation();
            }
        }

        if (GameStateManager.Instance.GameLoop.Paused && !_player.Input.CinematicMovement)
        {
            Animator.speed = 0;
        } else
        {
            Animator.speed = 1;
        }
    }
    /// <summary>
    /// Check which animation state machine we shouls use
    /// </summary>
    private void CheckAnimationStance()
    {
        if (_player.Movement.Casting && !_player.AbilityController._allAbilitiesDisabled)
            SetAnimationStance(AnimationStance.Casting);
        else if (_player.Movement.Shooting)
            SetAnimationStance(AnimationStance.Aiming);
        else
            SetAnimationStance(AnimationStance.Idle);
    }

    /// <summary>
    /// Check which animation in Aiming substatemachine we should use
    /// </summary>
    private void CheckAimAnimation()
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

    /// <summary>
    /// Check which animation in Casting substatemachine we should use
    /// </summary>
    private void CheckCastAnimation()
    {        
            if (CastOnce)
            {
                SetAnimation(AnimationState.Cast);
                CastOnce = false;
                _stillCasting = true;
                StartCoroutine(ResetCastIdle());
            }
            else if (!Moving)
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

    /// <summary>
    /// Which animation in Idle statemachine we should use
    /// </summary>
    private void CheckIdleAnimation()
    {
        if (Moving)
            SetAnimation(AnimationState.Run);
        else
            SetAnimation(AnimationState.Idle);
    }    

    /// <summary>
    /// resets to Idle animation after single cast animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetCastIdle()
    {
        yield return new WaitForSeconds(CastTime);
        _stillCasting = false;
        SetAnimation(AnimationState.Idle);                
    }

    /// <summary>
    /// Changes the animation stance if it's not already same
    /// </summary>
    /// <param name="stance"></param>
    private void SetAnimationStance(AnimationStance stance)
    {
        if (stance != _currentStance)
        {
            StopAllCoroutines();
            _currentStance = stance;
            Animator.SetInteger("stance", (int)stance);
        }
    }

    /// <summary>
    /// Changes the animation state if it's not already same
    /// </summary>
    /// <param name="animation"></param>
    public void SetAnimation(AnimationState animation)
    {
        if(animation != _currentState)
        {
            _currentState = animation;
            Animator.SetInteger("animState", (int)animation);
        }        
    }

    public void OnEnable()
    {
        Animator.enabled = true;
    }

    public void OnDisable()
    {
        Animator.enabled = false;
    }
}
