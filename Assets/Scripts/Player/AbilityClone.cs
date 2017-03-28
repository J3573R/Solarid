using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityClone : AbilityBase {
       
    private Vector3 _targetPosition;
    private Player _player;

	// Use this for initialization
	void Awake () {
        _player = GetComponent<Player>();
	}

    /// <summary>
    /// Performs the blink on the position of the mouse. 
    /// </summary>
    public override void Execute(Vector3 targetPos)
    {
        _targetPosition = targetPos;     
                          
    }
}
