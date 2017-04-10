using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    // The one and only player
    public static GameObject Player;
    public static InputController InputController;
    // Player damage
    // TODO: Transfer this to player or gun or something
    public static int PlayerDamage = 80;
    // Interact button is or is not pressed
    public static bool Interact;

    public static bool Paused;

    public static CameraFollow CameraScript;
}
