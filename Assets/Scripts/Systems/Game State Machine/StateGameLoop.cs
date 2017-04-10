using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateGameLoop : GameStateBase
{
    public Text GitGud;
    public Player Player;
    public bool EnemiesReady;
    public bool PlayerReady;
    public bool CameraReady;

    private bool _gameInitialized;
    private Camera _camera;
    private HudController _hud;
    private bool _dying = false;


    protected override void Awake()
    {
        base.Awake();
        LevelName = SceneManager.GetActiveScene().name;
        GitGud = GameObject.Find("UI/Git_gud").GetComponent<Text>();
        GitGud.gameObject.SetActive(false);
        _hud = GameObject.Find("HUD").GetComponent<HudController>();
        _hud.init();
        
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
        _hud.FadeScreenToVisible();
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

        if (CameraReady && PlayerReady && CameraReady && !_gameInitialized)
        {
            _hud.FadeScreenToVisible();
            Globals.Paused = false;
            _gameInitialized = true;
            Debug.Log("AllReady");
        }
    }

    public IEnumerator Die()
    {        
        yield return new WaitForSeconds(2);
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, LevelName);
    }
}
