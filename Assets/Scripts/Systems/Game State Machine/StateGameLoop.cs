using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateGameLoop : GameStateBase
{
    public Text GitGud;
    public Player Player;
    public GameLoopReferences References = new GameLoopReferences();
    public bool EnemiesReady;
    public bool PlayerReady;
    public bool CameraReady;
    public bool Paused;

    private InputController InputController;
    private bool _gameInitialized;
    private Camera _camera;
    private HudController _hud;
    private bool _dying = false;


    protected override void Awake()
    {
        GameStateManager.Instance.GameLoop.References.CameraScript = FindObjectOfType<CameraFollow>();
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

        Player = GameStateManager.Instance.GameLoop.Player;

        Player = GameObject.FindObjectOfType<Player>();
        InputController = Player.GetComponent<InputController>();
        //_hud.FadeScreenToVisible();
        References.Init();

    }

    protected override void Update()
    {
        if (!_dying && Player.Dead)
        {                        
            _dying = true;
            GitGud.gameObject.SetActive(true);
            InputController.ListenInput = false;
            StartCoroutine(Die());
        }

        if (CameraReady && PlayerReady && CameraReady && !_gameInitialized)
        {
            GameStateManager.Instance.FadeScreenToVisible(2);
            GameStateManager.Instance.GameLoop.Paused = false;

            //_hud.FadeScreenToVisible();
            Paused = true;

            _gameInitialized = true;
        }
    }

    public IEnumerator Die()
    {        
        yield return new WaitForSeconds(2);
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, LevelName);
    }
}
