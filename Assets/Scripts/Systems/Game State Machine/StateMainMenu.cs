using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StateMainMenu : GameStateBase {

    private Button _buttonNewGame;
    private Button _buttonOptions;
    private Button _buttonExit;
    private Button _buttonContinue;
    private Button _buttonReset;
    private Button _buttonYes;
    private Button _buttonNo;

    private bool _saveExists;
    private GameObject _warningPanel;
    

    protected override void Awake()
    {
        base.Awake();
        LevelName = "MainMenu";
        _buttonNewGame = GameObject.Find("ButtonNewGame").GetComponent<Button>();        
        _buttonOptions = GameObject.Find("ButtonOptions").GetComponent<Button>();
        _buttonExit = GameObject.Find("ButtonExit").GetComponent<Button>();
        _buttonContinue = GameObject.Find("ButtonContinue").GetComponent<Button>();
        _buttonReset = GameObject.Find("ButtonReset").GetComponent<Button>();
        _warningPanel = GameObject.Find("WarningPanel");
        _buttonYes = GameObject.Find("ButtonYes").GetComponent<Button>();
        _buttonNo = GameObject.Find("ButtonNo").GetComponent<Button>();

        _warningPanel.SetActive(false);

        _buttonNewGame.onClick.AddListener(PressNewGame);
        _buttonOptions.onClick.AddListener(PressOptions);
        _buttonExit.onClick.AddListener(PressExit);
        _buttonReset.onClick.AddListener(PressReset);

        if (SaveSystem.Instance == null)
        {
            GameStateManager.Instance.SetupSaveSystem();
        }
        if (SaveSystem.Instance.SaveData.GetCurrentLevel() == SaveData.Level.NoSave)
        {
            GameObject.Find("ButtonContinue").SetActive(false);
            _saveExists = false;
        }
        else
        {
            _buttonContinue.onClick.AddListener(PressContinue);
            _saveExists = true;
        }

        GameStateManager.Instance.FadeScreenToVisible(1);
    }

    private void PressReset()
    {
        SaveSystem.Instance.SaveData.SetCurrentLevel(SaveData.Level.NoSave);
        SaveSystem.Instance.SaveToFile();
    }

    void PressNewGame()
    {
        if (!_saveExists)
        {
            GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, "South1");
        }                      
        else
        {
            _warningPanel.SetActive(true);
            _buttonContinue.gameObject.SetActive(false);
            _buttonExit.gameObject.SetActive(false);
            _buttonNewGame.gameObject.SetActive(false);
            _buttonOptions.gameObject.SetActive(false);
            _buttonReset.gameObject.SetActive(false);

            _buttonYes.onClick.AddListener(PressYes);
            _buttonNo.onClick.AddListener(PressNo);
        }            
    }

    private void PressNo()
    {
        _warningPanel.SetActive(false);
        _buttonContinue.gameObject.SetActive(true);
        _buttonExit.gameObject.SetActive(true);
        _buttonNewGame.gameObject.SetActive(true);
        _buttonOptions.gameObject.SetActive(true);
        _buttonReset.gameObject.SetActive(true);
    }

    private void PressYes()
    {
        SaveSystem.Instance.SaveData.SetCurrentLevel(SaveData.Level.NoSave);
        SaveSystem.Instance.SaveToFile();
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, "South1");
    }

    void PressOptions()
    {
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.Options, "Options");
    }

    void PressExit()
    {
        Application.Quit();
    }

    private void PressContinue()
    {
        string levelName = SaveSystem.Instance.SaveData.GetCurrentLevel().ToString();
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, levelName);
    }
}
