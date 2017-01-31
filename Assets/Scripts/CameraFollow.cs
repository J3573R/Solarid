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

        Vector3 tmp = new Vector3(_player.transform.position.x, 10f, _player.transform.position.z - 5);
        transform.position = Vector3.MoveTowards(transform.position, tmp, Time.deltaTime * 20);
    }
}
