using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOptions : GameStateBase {

    protected override void Awake()
    {
        base.Awake();
        LevelName = "Options";        
    }
}
