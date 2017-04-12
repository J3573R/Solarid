﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSplashScreen : GameStateBase {

    public float TimeToSwitch = 3f;

    private float _switchTimer = 0;

    protected override void Awake()
    {
        base.Awake();
        LevelName = "SplashScreen";
        GameStateManager.Instance.FadeScreenToVisible(1);
    }


    protected override void Update()
    {
        _switchTimer += Time.deltaTime;

        // Change scene to main menu when time is up or any key is pressed
        if(_switchTimer > TimeToSwitch || Input.anyKey)
        {
            GameStateManager.Instance.ChangeState(GameStateManager.GameState.MainMenu, "MainMenu");
        }

    }
}
