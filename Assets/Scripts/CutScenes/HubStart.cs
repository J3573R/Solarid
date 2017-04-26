using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubStart : MonoBehaviour {

    [SerializeField]
    private GameObject _blueCrystal;
    [SerializeField]
    private GameObject _redCrystal;
    [SerializeField]
    private GameObject _yellowCrystal;
    [SerializeField]
    private GameObject _blackCrystal;

    private Image _blackScreen;
    private Dictionary<SaveData.Crystal, bool> _hubCrystals;
    private SaveData.Crystal _crystalWithPlayer;

	// Use this for initialization
	void Start () {
        _hubCrystals = SaveSystem.Instance.SaveData.GetHubCrystals();
        _crystalWithPlayer = SaveSystem.Instance.SaveData.GetCrystalWithPlayer();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();

        SetupHub();

        _blackScreen.CrossFadeAlpha(0, 5, true);
    }

    private void SetupHub()
    {
        if (_hubCrystals[SaveData.Crystal.Blue])
        {
            _blueCrystal.SetActive(true);
        }
        if (_hubCrystals[SaveData.Crystal.Red])
        {
            _redCrystal.SetActive(true);
        }
        if (_hubCrystals[SaveData.Crystal.Yellow])
        {
            _yellowCrystal.SetActive(true);
        }
        if (_hubCrystals[SaveData.Crystal.Black])
        {
            _blackCrystal.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
