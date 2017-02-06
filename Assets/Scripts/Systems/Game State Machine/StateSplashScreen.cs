using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSplashScreen : GameStateBase {

    private float _switchTimer = 0;

    void Awake()
    {
        LevelName = "SplashScreen";
    }

    protected override void Update()
    {

    }
}
