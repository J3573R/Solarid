using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Speed of camera delay when following player
    public float CameraDelay = 5f;
    // Camera offset from player
    public Vector3 CameraOffset = new Vector3(0, 10, -5);
    public MonoBehaviour AnimationComponent;
    public Vector3 MouseOffset = Vector3.zero;
    public bool Initialized = false;

    // Players gameobject
    private GameObject _player;
    // Cameras current position
    private Vector3 _vCurPos;
    

    private Quaternion _originalRotation;
    private bool shake = false;
    private float shakeAmount = 0;
    private float shakeDuration = 0;
    private float originalShakeDuration;

    public bool StopNormalCameraMovement { get; set; }

    public void Init()
    {

        if (!Initialized)
        {           
            _player = GameObject.Find("Player");
            GameStateManager.Instance.GameLoop.References.CameraScript = this;
            transform.position = _player.transform.position + CameraOffset;
            Initialized = true;
            _originalRotation = transform.rotation;
            
            GameStateManager.Instance.GameLoop.CameraReady = true;
            _player = GameObject.Find("Player");
        }
        
    }

    public void ResetCamera(bool toPlayerPos = true)
    {
        StopNormalCameraMovement = false;
        transform.rotation = _originalRotation;

        if (toPlayerPos)
            transform.position = _player.transform.position + CameraOffset;
    }
    
    public void AnimationCompleted()
    {
        if (AnimationComponent != null)
            AnimationComponent.SendMessage("AnimationCompleted");
    }

    void Update()
    {
        if (!Initialized && GameStateManager.Instance.GameLoop != null)
        {
            Init();
        }

        if (Initialized)
        {
            if (shake)
            {
                if (shakeDuration > 0)
                {
                    _vCurPos = _player.transform.position + CameraOffset;
                    var time = Time.smoothDeltaTime * CameraDelay;
                    _vCurPos = Vector3.Lerp(transform.position, _vCurPos + MouseOffset, time);
                    transform.position = _vCurPos + Random.insideUnitSphere * Mathf.Lerp(0, shakeAmount, shakeDuration / originalShakeDuration);
                }
                else
                {
                    shake = false;
                }
                shakeDuration -= Time.deltaTime;
            }
        }
        
    }

    void FixedUpdate()
    {
        if (!StopNormalCameraMovement)
        {
            if (Initialized)
            {
                _vCurPos = _player.transform.position + CameraOffset;
                var time = Time.smoothDeltaTime * CameraDelay * 2;
                _vCurPos = Vector3.Lerp(transform.position, _vCurPos + MouseOffset, time);
                transform.position = _vCurPos;
            }
        }
        
        
        /*if(MouseOffset == Vector3.zero)
        {
            _vCurPos = _player.transform.position + CameraOffset;
            var time = Time.smoothDeltaTime * CameraDelay;
            transform.position = Vector3.Lerp(transform.position, _vCurPos, time);
        } else
        {
            _vCurPos = _player.transform.position + CameraOffset + MouseOffset;
            transform.position = Vector3.Lerp(transform.position, _vCurPos, Time.smoothDeltaTime);
        }*/
    }

    public void Harlem(float amount, float duration)
    {
        shakeAmount = amount;
        shakeDuration = duration;
        originalShakeDuration = duration;
        shake = true;
    }

    public void AddMouseOffset(Vector3 offset)
    {
        Vector3 direction = (offset - _player.transform.position).normalized;        
        direction.y = 0;
        MouseOffset = direction * 2;
    }
}
