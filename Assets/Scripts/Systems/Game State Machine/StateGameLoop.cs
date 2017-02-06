using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateGameLoop : GameStateBase
{
    void Awake()
    {
        LevelName = SceneManager.GetActiveScene().name;
    }

    protected override void Update()
    {
        Debug.Log("Running GameLoop Code at:" + LevelName);
    }
}
