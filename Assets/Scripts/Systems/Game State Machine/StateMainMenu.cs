using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMainMenu : GameStateBase {

    private Button _buttonPlay;
    private Button _buttonOptions;
    private Button _buttonExit;

    void Awake()
    {
        LevelName = "MainMenu";
        _buttonPlay = GameObject.Find("ButtonPlay").GetComponent<Button>();        
        _buttonOptions = GameObject.Find("ButtonOptions").GetComponent<Button>();
        _buttonExit = GameObject.Find("ButtonExit").GetComponent<Button>();

        _buttonPlay.onClick.AddListener(PressPlay);
        _buttonOptions.onClick.AddListener(PressOptions);
        _buttonExit.onClick.AddListener(PressExit);
    }

    void PressPlay()
    {
        Debug.Log("PLAY PRESSED");
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, "Default");
    }

    void PressOptions()
    {
        Debug.Log("OPTIONS PRESSED");
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.Options, "Options");
    }

    void PressExit()
    {
        Debug.Log("EXIT PRESSED");
        Application.Quit();
    }
}
