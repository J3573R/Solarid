using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    // Door to open
    public Door TargetDoor;
    // Default slider to use
    public GameObject SliderPrefab;
    //Objects to activate when interacted
    public List<GameObject> ActivateObjects;

    // Sliderbar offset from switch
    [SerializeField] private Vector3 _offset = new Vector3(0, 1, 0);
    // Gameobject that has slider component
    private GameObject _sliderBar;
    // Slider component to control
    private Slider _slider;
    // Distance between switch and player
    private float _distance;
    // Value of slider bar
    private float _switchValue;

    private RotateConstantly _obeliskRotation;
    private Collider[] _platformColliders;
    private AudioSource _audio;
    

    public void Awake()
    {
        _obeliskRotation = GetComponentInChildren<RotateConstantly>();
        _sliderBar = Instantiate(SliderPrefab);
        _sliderBar.transform.SetParent(GameObject.Find("UI").transform);
        _slider = _sliderBar.GetComponent<Slider>();
        _switchValue = 0;
        _slider.transform.localScale = new Vector3(1f, 1f, 1f);
        _sliderBar.SetActive(false);
        _platformColliders = TargetDoor.gameObject.GetComponentsInChildren<Collider>();
        ToggleDoorColliders(TargetDoor.Open);
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (TargetDoor.Moving && _obeliskRotation != null)
        {
            _obeliskRotation.ChangeRotationSpeed = new Vector3(0, 0, 500);
        } else if(_obeliskRotation != null)
        {
            _obeliskRotation.ChangeRotationSpeed = new Vector3(0, 0, 30);
        }

        _distance = Vector3.Distance(GameStateManager.Instance.GameLoop.Player.gameObject.transform.position, transform.position);

        // If player is close enought and door is not moving, show meter and response to interaction
        if (_distance <= 2 && !TargetDoor.Moving && !TargetDoor.Open)
        {
            if (!_sliderBar.activeInHierarchy)
            {
                _sliderBar.SetActive(true);
            }

            _sliderBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + _offset);

            // If Interact button is pressed
            if (GameStateManager.Instance.GameLoop.References.Player.Interact)
            {
                _switchValue += Time.deltaTime * 1f;
                _slider.value = _switchValue;

                // Slider is on top
                if (_switchValue >= _slider.maxValue)
                {
                    if (ActivateObjects.Count > 0)
                    {
                        foreach (var currentObject in ActivateObjects)
                        {
                            currentObject.SetActive(true);
                        }
                    }
                    TargetDoor.ToggleDoor();
                    ToggleDoorColliders(TargetDoor.Open);
                    _audio.Play();
                }
            }
            else if (_switchValue > 0)
            {
                _switchValue -= Time.deltaTime * 2f;
                _slider.value = _switchValue;
            }
        }
        else
        {                  
            // Reset slider bar
            if (_sliderBar.activeInHierarchy)
            {
                _switchValue = 0;
                _slider.value = 0;
                _sliderBar.SetActive(false);                
            }
        }
        
    }

    void ToggleDoorColliders(bool state)
    {
        foreach (var collider in _platformColliders)
        {
            collider.enabled = state;
        }
    }
}
