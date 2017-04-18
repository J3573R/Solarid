using System;
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
    private Image _blackScreen;
    private GameObject _pauseMenu;
    private HudController _hud;
    private bool _dying = false;


    protected override void Awake()
    {
        References.CameraScript = FindObjectOfType<CameraFollow>();
        base.Awake();
        
        LevelName = SceneManager.GetActiveScene().name;
        GitGud = GameObject.Find("UI/Git_gud").GetComponent<Text>();
        GitGud.gameObject.SetActive(false);
        _hud = GameObject.Find("HUD").GetComponent<HudController>();
        _hud.init();
        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _pauseMenu = GameObject.Find("PauseMenu");
        _pauseMenu.SetActive(false);
        
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
        References.Init();

    }

    protected override void Update()
    {
        if (!Paused && Input.GetButtonDown("Pause"))        
            Pause();           
        else if (Paused && Input.GetButtonDown("Pause"))        
            UnPause();  
        
        if (!_dying && Player.Dead)
        {                        
            _dying = true;
            GitGud.gameObject.SetActive(true);
            InputController.ListenInput = false;
            StartCoroutine(Die());
        }

        if (CameraReady && PlayerReady && CameraReady && !_gameInitialized)
        {
            GameObject tmp = GameObject.Find("LevelStartSequence");

            if (tmp == null)
            {
                GameStateManager.Instance.FadeScreenToVisible(2);
                Paused = false;
            } else
            {
                MonoBehaviour[] components = tmp.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour co in components)
                {
                    co.enabled = true;
                }
            }
            _gameInitialized = true;
        }
    }

    public void UnPause()
    {
        Paused = false;
        _blackScreen.CrossFadeAlpha(0, 0, true);
        _pauseMenu.SetActive(false);
    }

    public void Pause(bool ActivatePauseMenu = true, bool FadeScreen = true)
    {
        Paused = true;               

        if (FadeScreen)
            _blackScreen.CrossFadeAlpha(0.5f, 0, true);

        if (ActivatePauseMenu)
            _pauseMenu.SetActive(true);
    }

    public IEnumerator Die()
    {        
        yield return new WaitForSeconds(2);
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameLoop, LevelName);
    }
}
