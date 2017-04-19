using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkPickupCutScene : MonoBehaviour {

    public GameObject Platform;

    private Door _door;

	// Use this for initialization
	void Start () {
        _door = Platform.GetComponent<Door>();
        _door.ToggleDoor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
