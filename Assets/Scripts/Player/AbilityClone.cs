using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityClone : AbilityBase {

    public GameObject ClonePrefab;
    public int ClonePoolSize;
    public List<GameObject> Clones = new List<GameObject>();

    private Vector3 _targetPosition;
    private Player _player;

	// Use this for initialization
	void Awake () {
        _player = GetComponent<Player>();
	    _player.Clones = Clones;
        for(int i = 0; i < ClonePoolSize; i++)
        {
            GameObject clone = Instantiate(ClonePrefab, transform.position, Quaternion.identity);
            clone.SetActive(false);
            Clones.Add(clone);
        }
	}

    /// <summary>
    /// Performs the blink on the position of the mouse. 
    /// </summary>
    public override void Execute(Vector3 targetPos)
    {
        _targetPosition = targetPos;        

        foreach(var clone in Clones)
        {
            if (!clone.activeInHierarchy)
            {
                _targetPosition.y = 0;
                clone.transform.position = _targetPosition;
                clone.SetActive(true);
                break;
            }
        }

        CoolDownRemaining = CoolDown;
    }
}
