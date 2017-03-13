﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCheck : MonoBehaviour {

    private MeshRenderer _renderer;
    private Player _player;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<MeshRenderer>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _renderer.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 tmp = _player.transform.position;
        tmp.y = 0.1f;
        transform.position = tmp;
	}

    public void DrawRange(float range, bool draw)
    {
        Vector3 tmp = new Vector3(range, 0.1f, range);
        transform.localScale = tmp;

        if (draw)
        {
            _renderer.enabled = true;
        } else
        {
            _renderer.enabled = false;
        }
    }
}
