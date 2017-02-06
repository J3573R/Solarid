using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameLoop : GameStateBase
{
    void Awake()
    {
        LevelName = "Default";
    }

    protected override void Update()
    {
        //Debug.Log("Running GameLoop Code");
    }
}
