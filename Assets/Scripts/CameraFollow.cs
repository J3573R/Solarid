using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject _player;

    void Awake()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        //transform.position = new Vector3(_player.transform.position.x, 10f, _player.transform.position.z - 5);
    }
}
