using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityClone : AbilityBase {

    public GameObject ClonePrefab;
    public int ClonePoolSize;

    private Vector3 _targetPosition;
    private Player _player;
    private List<GameObject> _clones = new List<GameObject>();

	// Use this for initialization
	void Awake () {
        _player = GetComponent<Player>();
        for(int i = 0; i < ClonePoolSize; i++)
        {
            GameObject clone = Instantiate(ClonePrefab, transform.position, Quaternion.identity);
            clone.SetActive(false);
            _clones.Add(clone);
        }
	}

    /// <summary>
    /// Performs the blink on the position of the mouse. 
    /// </summary>
    public override void Execute(Vector3 targetPos)
    {
        _targetPosition = targetPos;        

        foreach(var clone in _clones)
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
