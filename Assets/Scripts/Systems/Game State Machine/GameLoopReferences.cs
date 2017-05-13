using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameLoopReferences
{
    public Player Player;
    public CameraFollow CameraScript;

    public void Init()
    {
        Player = GameObject.FindObjectOfType<Player>();
        CameraScript = GameObject.FindObjectOfType<CameraFollow>();
    }
}
