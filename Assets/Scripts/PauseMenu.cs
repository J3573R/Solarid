using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    private Button _buttonBackToGame;
    [SerializeField]
    private Button _buttonMainMenu;
    [SerializeField]
    private Button _buttonExitGame;


	// Use this for initialization
	void Start () {
        _buttonBackToGame.onClick.AddListener(PressBackToGame);
        _buttonMainMenu.onClick.AddListener(PressMainMenu);
        _buttonExitGame.onClick.AddListener(PressExitGame);
    }

    private void PressExitGame()
    {
        Application.Quit();
    }

    private void PressMainMenu()
    {
        gameObject.SetActive(false);
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.MainMenu, "MainMenu");
    }

    private void PressBackToGame()
    {
        GameStateManager.Instance.GameLoop.UnPause();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
