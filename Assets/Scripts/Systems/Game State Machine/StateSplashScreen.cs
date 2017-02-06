using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSplashScreen : GameStateBase {

    public float TimeToSwitch = 3f;

    private float _switchTimer = 0;

    protected override void Awake()
    {
        base.Awake();
        LevelName = "SplashScreen";        
    }

    protected override void Update()
    {
        _switchTimer += Time.deltaTime;

        if(_switchTimer > TimeToSwitch || Input.anyKey)
        {
            GameStateManager.Instance.ChangeState(GameStateManager.GameState.MainMenu, "MainMenu");
        }

    }
}
