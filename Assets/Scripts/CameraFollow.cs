using UnityEngine;

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

    private bool shake = false;
    private float shakeAmount = 0;
    private float shakeDuration = 0;
    private float originalShakeDuration;
    private Vector3 _originalPosition;

    void Awake()
    {
        _player = GameObject.Find("Player");
        Globals.CameraScript = this;
    }

    void Update()
    {
        if (shake)
        {
            if(shakeDuration > 0)
            {
                transform.localPosition = _originalPosition + Random.insideUnitSphere * Mathf.Lerp(0, shakeAmount, shakeDuration / originalShakeDuration);                
            }
            else
            {
                shake = false;
            }
            shakeDuration -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        _vCurPos = _player.transform.position + CameraOffset;
        transform.position = Vector3.Lerp(transform.position, _vCurPos, Time.smoothDeltaTime * CameraDelay);
        //transform.position = new Vector3(_player.transform.position.x, 10f, _player.transform.position.z - 5);
        //Vector3 tmp = new Vector3(_player.transform.position.x, 10f, _player.transform.position.z - 5);
        //transform.position = Vector3.MoveTowards(transform.position, tmp, Time.deltaTime * 20);
    }

    public void Harlem(float amount, float duration)
    {
        _originalPosition = transform.position;
        shakeAmount = amount;
        shakeDuration = duration;
        originalShakeDuration = duration;
        shake = true;
    }
}
