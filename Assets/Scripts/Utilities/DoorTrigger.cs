using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

	public List<Transform> Doors;
	public bool LoweringDoor = false;

    private List<Door> _doorScripts;
	private List<LoweringDoor> _loweringDoorScripts;

	// Use this for initialization
	void Start () {
		_doorScripts = new List<Door> ();
		_loweringDoorScripts = new List<LoweringDoor> ();

		if (!LoweringDoor) 
		{
			foreach (Transform tr in Doors)
			{
				_doorScripts.Add(tr.GetComponent<Door>());
			}
		} else 
		{
			foreach (Transform tr in Doors)
			{
				_loweringDoorScripts.Add(tr.GetComponent<LoweringDoor>());
			}
		}				
	}	

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
			if (!LoweringDoor) {
				foreach (Door door in _doorScripts) {
					door.ToggleDoor ();
				}
			} else 
			{
				foreach (LoweringDoor door in _loweringDoorScripts) {
					door.ToggleDoor ();
				}
			}
			Destroy (gameObject);
        }
    }
}
