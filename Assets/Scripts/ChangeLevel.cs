﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevel : MonoBehaviour
{

    public string TargetScene;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            try
            {
                SaveSystem.Instance.SaveAll();
                GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, TargetScene);
            }
            catch (Exception e)
            {
                Debug.Log("ERROR CATCHED:");
                Debug.LogError(e.Message);
            }
        }
    }
}
