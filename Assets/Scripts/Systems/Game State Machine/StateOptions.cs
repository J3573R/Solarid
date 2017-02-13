using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateOptions : GameStateBase
{

    private Button _buttonBack;

    protected override void Awake()
    {
        base.Awake();
        LevelName = "Options";

        _buttonBack = GameObject.Find("ButtonBack").GetComponent<Button>();

        _buttonBack.onClick.AddListener(PressBack);

    }

    private void PressBack()
    {
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.MainMenu, "MainMenu");
    }
}
