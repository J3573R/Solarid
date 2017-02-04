using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{

    #region Singleton
    // Single instance of singleton
    public static GameStateManager Instance;

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
    }
    #endregion

    public enum GameState
    {
        SplashScreen,
        MainMenu,
        GameLoop
    }

    // Current game state
    [SerializeField] private GameState _gameState = GameState.SplashScreen;
    // Gameobject including game state script
    private GameObject _gameStateObj;

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
            SetState(state);
            SceneManager.LoadScene(scene);
        }
        else
        {
            SetState(state);
        }
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
            case GameState.GameLoop:
                _gameStateObj.AddComponent<StateGameLoop>();
                _gameStateObj.name = "Game State: GameLoop";
                break;
        }

        DontDestroyOnLoad(_gameStateObj);
    }
}
