using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{

    public Door TargetDoor;
    public GameObject SliderPrefab;

    [SerializeField] private Vector3 _offset = new Vector3(0, 1, 0);
    private GameObject _sliderBar;
    private Slider _slider;
    private float _distance;
    private float _time;
    

    public void Awake()
    {
        _sliderBar = Instantiate(SliderPrefab);
        _sliderBar.transform.SetParent(GameObject.Find("UI").transform);
        _slider = _sliderBar.GetComponent<Slider>();
        _time = 0;
        _sliderBar.SetActive(false);
    }

    void Update()
    {
        _distance = Vector3.Distance(Globals.Player.transform.position, transform.position);

        if (_distance <= 2 && !TargetDoor.Moving)
        {
            if (Globals.Interact)
            {
                _time += Time.deltaTime * 1f;
                _slider.value = _time;
                if (_time >= _slider.maxValue)
                {
                    TargetDoor.ToggleDoor();
                }
            }
            else if(_time > 0)
            {
                _time -= Time.deltaTime * 2f;
                _slider.value = _time;
            }

            if (!_sliderBar.activeInHierarchy)
            {
                _sliderBar.SetActive(true);
            }

            _sliderBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + _offset);
        }
        else
        {
            if (_sliderBar.activeInHierarchy)
            {
                _time = 0;
                _slider.value = 0;
                _sliderBar.SetActive(false);
            }
        }
        
    }
}
