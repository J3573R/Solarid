using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{

    #region Singleton
    // Single instance of singleton
    public static GameStateManager Instance;
    [SerializeField] private GameObject _saveSystemPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
        SetupSaveSystem();
    }
    
    #endregion

    public enum GameState
    {
        SplashScreen,
        MainMenu,
        Options,
        Credits,
        GameLoop
    }

    private Image _blackScreen;
    public GameStateBase CurrentState;

    public StateGameLoop GameLoop;

    // Current game state
    [SerializeField] private GameState _gameState = GameState.SplashScreen;
    // Gameobject including game state script
    private GameObject _gameStateObj;

    public void SetupSaveSystem()
    {
        GameObject go = GameObject.Find("SaveSystem");
        if (go == null)
        {
            Instantiate(_saveSystemPrefab);            
        }
    }

    /// <summary>
    /// Changes or sets game state.
    /// </summary>
    /// <param name="state">Target state to change</param>
    /// <param name="scene">Target scene to load</param>
    public void ChangeState(GameState state, string scene = null)
    {
        Destroy(_gameStateObj);
        if (scene != null)
        {
            FadeScreenToBlack(1);
            StartCoroutine(SceneDelay(1, state, scene));            
        }
        else
        {
            SetState(state);
        }
    }

    private IEnumerator SceneDelay(float howLong, GameState state, string scene = null)
    {
        yield return new WaitForSeconds(howLong);
        _gameState = state;
        SceneManager.sceneLoaded += sceneLoaded;
        SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// Initialize gameobject and game state if doesn't exist
    /// </summary>
    private void Init()
    {
        if (_gameStateObj == null)
        {
            SetState(_gameState);
        }

        _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        _blackScreen.enabled = true;
    }

    /// <summary>
    /// Creates gameobject for game state and adds right component for it
    /// </summary>
    /// <param name="state">Game state to create</param>
    private void SetState(GameState state)
    {
        _gameStateObj = new GameObject();

        switch (state)
        {
            case GameState.SplashScreen:
                _gameStateObj.AddComponent<StateSplashScreen>();
                _gameStateObj.name = "Game State: SplashScreen";                
                break;
            case GameState.MainMenu:
                _gameStateObj.AddComponent<StateMainMenu>();
                _gameStateObj.name = "Game State: MainMenu";
                break;
            case GameState.Options:
                _gameStateObj.AddComponent<StateOptions>();
                _gameStateObj.name = "Game State: Options";
                break;
            case GameState.Credits:
                _gameStateObj.AddComponent<StateCredits>();
                _gameStateObj.name = "Game State: Credits";
                break;
            case GameState.GameLoop:
                GameLoop = _gameStateObj.AddComponent<StateGameLoop>();
                _gameStateObj.name = "Game State: GameLoop";
                break;
        }
        DontDestroyOnLoad(_gameStateObj);
    }

    /// <summary>
    /// Creates and initialize correct state AFTER scene is loaded.
    /// </summary>
    private void sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SetState(_gameState);
        SceneManager.sceneLoaded -= sceneLoaded;
    }

    /// <summary>
    /// Fades the screen to black
    /// </summary>
    public void FadeScreenToBlack(float time)
    {        
        if (_blackScreen != null)
        {
            _blackScreen.CrossFadeAlpha(1, time, true);
        }
        else
        {
            _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
            _blackScreen.enabled = true;
            _blackScreen.CrossFadeAlpha(1, time, true);
        }

    }

    /// <summary>
    /// Fades the screen to visible
    /// </summary>
    public void FadeScreenToVisible(float time)
    {
        if (_blackScreen != null)
        {
            _blackScreen.CrossFadeAlpha(0, time, true);
        }
        else
        {
            _blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
            _blackScreen.enabled = true;
            _blackScreen.CrossFadeAlpha(0, time, true);
        }
    }
}
