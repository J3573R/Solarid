using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    public List<GameObject> Doors;

    private List<Door> _doorScripts;

	// Use this for initialization
	void Start () {

		foreach (GameObject go in Doors)
        {
            _doorScripts.Add(go.GetComponent<Door>());
        }
	}
	

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Door door in _doorScripts)
            {
                door.ToggleDoor();
            }
        }
    }
}
