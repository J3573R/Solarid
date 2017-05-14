using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateOptions : GameStateBase
{

    private Button _buttonBack;
    private Button _buttonFps;
    private Text _buttonFpsOption;
    private Button _buttonSounds;
    private Text _buttonSoundsOption;
    private SaveSystem _saveSystem;

    protected override void Awake()
    {
        base.Awake();
        LevelName = "Options";

        _buttonBack = GameObject.Find("ButtonBack").GetComponent<Button>();
        _buttonFps = GameObject.Find("Toggle FPS").GetComponent<Button>();
        _buttonSounds = GameObject.Find("Toggle Sounds").GetComponent<Button>();

        _buttonFpsOption = GameObject.Find("Toggle FPS/Option").GetComponent<Text>();
        _buttonSoundsOption = GameObject.Find("Toggle Sounds/Option").GetComponent<Text>();

        _buttonBack.onClick.AddListener(PressBack);
        _buttonFps.onClick.AddListener(ToggleFps);
        _buttonSounds.onClick.AddListener(ToggleSounds);

        GameStateManager.Instance.FadeScreenToVisible(1);
    }

    void Start()
    {
        _saveSystem = SaveSystem.Instance;
        SetFpsOption();
        SetSoundsOption();
    }

    private void ToggleFps()
    {
        bool value = !_saveSystem.SaveData.FpsMeter;
        _saveSystem.SaveData.FpsMeter = value;
        _saveSystem.SaveToFile();
        SetFpsOption();
    }

    private void ToggleSounds()
    {
        bool value = !_saveSystem.SaveData.Sounds;
        _saveSystem.SaveData.Sounds = value;
        _saveSystem.SaveToFile();
        SetSoundsOption();
    }

    private void SetFpsOption()
    {
        if (_saveSystem.SaveData.FpsMeter)
        {
            _buttonFpsOption.text = "ON";
        }
        else
        {
            _buttonFpsOption.text = "OFF";
        }
    }

    private void SetSoundsOption()
    {
        if (_saveSystem.SaveData.Sounds)
        {
            _buttonSoundsOption.text = "ON";
            AudioListener.volume = 1;
        }
        else
        {
            _buttonSoundsOption.text = "OFF";
            AudioListener.volume = 0;
        }
    }

    private void PressBack()
    {
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.MainMenu, "MainMenu");
    }
}
