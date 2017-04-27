using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubStart : MonoBehaviour {

    [SerializeField]
    private GameObject _crystalBlue;
    [SerializeField]
    private GameObject _crystalRed;
    [SerializeField]
    private GameObject _crystalYellow;
    [SerializeField]
    private GameObject _crystalBlack;

    [SerializeField]
    private GameObject _switchBlue;
    [SerializeField]
    private GameObject _switchRed;
    [SerializeField]
    private GameObject _switchYellow;
    [SerializeField]
    private GameObject _switchBlack;

    [SerializeField]
    private Transform _startBlue;
    [SerializeField]
    private Transform _startRed;
    [SerializeField]
    private Transform _startYellow;
    [SerializeField]
    private Transform _startBlack;
        
    [SerializeField]
    private GameObject _barrierRed;
    [SerializeField]
    private GameObject _barrierYellow;
    [SerializeField]
    private GameObject _barrierBlack;

    private Player _player;
    private Image _blackScreen;
    private SaveData.HubState _hubState;
    private SaveData.Crystal _crystalWithPlayer;

	// Use this for initialization
	void Start () {
        _hubState = SaveSystem.Instance.SaveData.GetHubState();
        _crystalWithPlayer = SaveSystem.Instance.SaveData.GetCrystalWithPlayer();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _player = FindObjectOfType<Player>();
        SetupHub();

        _blackScreen.CrossFadeAlpha(0, 5, true);
    }

    private void SetupHub()
    {
        if (_hubState == SaveData.HubState.NothingActivated)
        {            
            _switchBlack.SetActive(false);
            _switchRed.SetActive(false);
            _switchYellow.SetActive(false);
            _player.transform.position = _startBlue.position;
            _player.transform.rotation = _startBlue.rotation;            

        } else if (_hubState == SaveData.HubState.BlueActivated)
        {
            _switchBlue.SetActive(false);
            _switchBlack.SetActive(false);
            _switchYellow.SetActive(false);
            _crystalBlue.SetActive(true);
            _player.transform.position = _startRed.position;
            _player.transform.rotation = _startRed.rotation;
            _barrierRed.SetActive(false);

            if (SaveSystem.Instance.SaveData.GetCrystalWithPlayer() == SaveData.Crystal.none)
            {
                _switchRed.SetActive(false);
            }
        }
        else if (_hubState == SaveData.HubState.BlueRedActivated)
        {
            _switchBlue.SetActive(false);
            _switchBlack.SetActive(false);
            _switchRed.SetActive(false);
            _crystalBlue.SetActive(true);
            _crystalRed.SetActive(true);
            _player.transform.position = _startYellow.position;
            _player.transform.rotation = _startYellow.rotation;
            _barrierRed.SetActive(false);
            _barrierYellow.SetActive(false);

            if (SaveSystem.Instance.SaveData.GetCrystalWithPlayer() == SaveData.Crystal.none)
            {
                _switchYellow.SetActive(false);
            }
        }
        else if (_hubState == SaveData.HubState.BlueRedYellowActivated)
        {
            _switchBlue.SetActive(false);
            _switchRed.SetActive(false);
            _switchYellow.SetActive(false);
            _crystalBlue.SetActive(true);
            _crystalRed.SetActive(true);
            _crystalYellow.SetActive(true);
            _player.transform.position = _startBlack.position;
            _player.transform.rotation = _startBlack.rotation;
            _barrierRed.SetActive(false);
            _barrierYellow.SetActive(false);
            _barrierBlack.SetActive(false);
        }       

    }

    // Update is called once per frame
    void Update () {
		
	}
}
