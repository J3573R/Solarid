using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateGameLoop : GameStateBase
{
    public Text GitGud;
    public Player Player;

    private bool _dying = false;

    protected override void Awake()
    {
        base.Awake();
        LevelName = SceneManager.GetActiveScene().name;
        GitGud = GameObject.Find("UI/Git_gud").GetComponent<Text>();
        GitGud.gameObject.SetActive(false);
        
        if (SaveSystem.Instance != null)
            SaveSystem.Instance.SaveCurrentLevel();
        else
        {
            GameStateManager.Instance.SetupSaveSystem();
            SaveSystem.Instance.SaveCurrentLevel();
        }
    }

    void Start()
    {
        Player = Globals.Player.GetComponent<Player>();        
    }

    protected override void Update()
    {
        if (!_dying && Player.Dead)
        {                        
            _dying = true;
            GitGud.gameObject.SetActive(true);
            Globals.InputController.ListenInput = false;
            StartCoroutine(Die());

        }
        //Debug.Log("Running GameLoop Code at:" + LevelName);
    }

    public IEnumerator Die()
    {        
        yield return new WaitForSeconds(2);
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, LevelName);
    }
}
