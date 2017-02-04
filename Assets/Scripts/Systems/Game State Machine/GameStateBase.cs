using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBase : MonoBehaviour
{

    public string LevelName = "BaseClass";

    protected virtual void OnEnable()
    {
        Debug.Log("State Enabled: " + LevelName);
    }

    protected virtual void Update()
    {
        Debug.Log("State running: " + LevelName);
    }

    protected virtual void OnDisable()
    {
        Debug.Log("State Disabled: " + LevelName);
    }
}
