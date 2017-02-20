﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    // Door to open
    public Door TargetDoor;
    // Default slider to use
    public GameObject SliderPrefab;

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
    

    public void Awake()
    {
        _sliderBar = Instantiate(SliderPrefab);
        _sliderBar.transform.SetParent(GameObject.Find("UI").transform);
        _slider = _sliderBar.GetComponent<Slider>();
        _switchValue = 0;
        _sliderBar.SetActive(false);
    }

    void Update()
    {
        _distance = Vector3.Distance(Globals.Player.transform.position, transform.position);

        // If player is close enought and door is not moving, show meter and response to interaction
        if (_distance <= 2 && !TargetDoor.Moving)
        {
            if (!_sliderBar.activeInHierarchy)
            {
                _sliderBar.SetActive(true);
            }

            _sliderBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + _offset);

            // If Interact button is pressed
            if (Globals.Interact)
            {
                _switchValue += Time.deltaTime * 1f;
                _slider.value = _switchValue;

                // Slider is on top
                if (_switchValue >= _slider.maxValue)
                {
                    TargetDoor.ToggleDoor();
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
}