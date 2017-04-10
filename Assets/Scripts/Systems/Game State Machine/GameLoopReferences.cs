using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameLoopReferences
{
    public Player Player;
    public StateGameLoop GameLoop;
    public ManaExplosion ManaExplosion;
    public CameraFollow CameraScript;

    public void Init()
    {
        Player = GameObject.FindObjectOfType<Player>();
        ManaExplosion = GameObject.FindObjectOfType<ManaExplosion>();
        CameraScript = GameObject.FindObjectOfType<CameraFollow>();
    }
}
