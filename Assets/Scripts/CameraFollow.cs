﻿using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Speed of camera delay when following player
    public float CameraDelay = 5f;
    // Camera offset from player
    public Vector3 CameraOffset = new Vector3(0, 10, -5);

    // Players gameobject
    private GameObject _player;
    // Cameras current position
    private Vector3 _vCurPos; 

    void Awake()
    {
        _player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
<<<<<<< HEAD
        _vCurPos = _player.transform.position + CameraOffset;
        transform.position = Vector3.Lerp(transform.position, _vCurPos, Time.smoothDeltaTime * CameraDelay);
=======
        //transform.position = new Vector3(_player.transform.position.x, 10f, _player.transform.position.z - 5);

        Vector3 tmp = new Vector3(_player.transform.position.x, 10f, _player.transform.position.z - 5);
        transform.position = Vector3.MoveTowards(transform.position, tmp, Time.deltaTime * 20);
>>>>>>> Teemun_devi
    }
}
