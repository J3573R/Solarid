using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBase : MonoBehaviour
{

    public string LevelName = "BaseClass";

    /// <summary>
    /// Fix for lightning map bake bug
    /// </summary>
    protected virtual void Awake()
    {
        #if UNITY_EDITOR
        if (UnityEditor.Lightmapping.giWorkflowMode == UnityEditor.Lightmapping.GIWorkflowMode.Iterative)
                {
                    DynamicGI.UpdateEnvironment();
                }
        #endif
    }

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
