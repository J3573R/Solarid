using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMainMenu : GameStateBase {

    private Button _buttonPlay;
    private Button _buttonOptions;
    private Button _buttonExit;

    protected override void Awake()
    {
        base.Awake();
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
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, "Level1");
    }

    void PressOptions()
    {
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.Options, "Options");
    }

    void PressExit()
    {
        Application.Quit();
    }
}
