
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


[RequireComponent(typeof(AudioSource))]

public class GameProcess : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _spawn;

    [SerializeField] private GameObject _menuGameStarterOrEnderPrefab;
    [SerializeField] private GameObject _menuGameResumerPrefab;
    [SerializeField] private GameObject _meniGameEnderOrRestarterPrefab;

    [SerializeField] private AudioClip _clipRun;
    [SerializeField] private AudioClip _clipLoose;
    [SerializeField] private AudioClip _clipWinner;

    [SerializeField] private AudioSource _environmentSound;
    [SerializeField] private AudioSource _backgroundMusic;

    private GameObject _menuGameStarterOrEnder;
    private GameObject _menuGameResumer;
    private GameObject _meniGameEnderOrRestarter;

    public GameObject GameStarter => _menuGameStarterOrEnder;
    public GameObject GameResumer => _menuGameResumer;
    public GameObject GameEnderOrRestarter => _meniGameEnderOrRestarter;

    private GameStartOrEnd _gameStarterComponent;
    private GameRestartOrEnd _gameEnderComponent;
    private GameResume _gameResumerComponent;
    private Spawner _botSpawnComponent;
    private Player _playerComponent;

    public event UnityAction GameStart;

    public const int GAME = 0;
    public const int MENU = 1;
    public const int PAUSE = 2;
    public const int WINNER = 3;
    public const int LOSE = 4;

    public int GameState { private set; get; } = MENU;

    public void OnGameStart() 
    { 
        GameStart?.Invoke();
    }

    private void ChangeAndRunSound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    private void createMenuBox<T>(ref GameObject box, ref GameObject prefab, ref T component)
    {
        box = Instantiate(prefab, _canvas.transform);
        component = box.GetComponent<T>();
    }

    private void Awake()
    {
        createMenuBox(ref _menuGameStarterOrEnder, ref _menuGameStarterOrEnderPrefab, ref _gameStarterComponent);

        _botSpawnComponent = _spawn.GetComponent<Spawner>();
        _environmentSound = GetComponent<AudioSource>();
        _playerComponent = _player.GetComponent<Player>();
    }

    private void OnEnable()
    {
        _gameStarterComponent.GameRun += OnGameRun;
        _gameStarterComponent.GameDone += OnGameDone;
        _botSpawnComponent.Winner += OnWinnder;
        _playerComponent.Dead += OnLoose;
    }

    private void OnDisable()
    {
        _botSpawnComponent.Winner -= OnWinnder;
        _playerComponent.Dead -= OnLoose;

        if (_gameEnderComponent is GameRestartOrEnd)
        {
            _gameEnderComponent.GameRestart -= OnGameRestart;
            _gameEnderComponent.GameDone -= OnGameDone;
        }

        if (_gameResumerComponent is GameResume)
        {
            _gameResumerComponent.ResumeGame -= OnGameResume;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameState == GAME)
            {
                GameState = PAUSE;

                createMenuBox(ref _menuGameResumer, ref _menuGameResumerPrefab, ref _gameResumerComponent);

                _gameResumerComponent.ResumeGame += OnGameResume;
            }
        }
    }

    private void OnGameResume()
    {
        if (GameState == MENU)
        {
            OnGameStart();
        }

        GameState = GAME;
        Destroy(_menuGameResumer);
    }

    private void OnWinnder()
    {
        GameState = WINNER;

        createMenuBox(ref _meniGameEnderOrRestarter, ref _meniGameEnderOrRestarterPrefab, ref _gameEnderComponent);

        _gameEnderComponent.GameRestart += OnGameRestart;
        _gameEnderComponent.GameDone += OnGameDone;

        ChangeAndRunSound(_environmentSound, _clipWinner);
    }

    private void OnGameRun()
    {
        if (GameState == MENU) 
        { 
            OnGameStart(); 
        }

        GameState = GAME;

        _gameStarterComponent.GameRun -= OnGameRun;
        _gameStarterComponent.GameDone -= OnGameDone;

        Destroy(_menuGameStarterOrEnder);

        ChangeAndRunSound(_environmentSound, _clipRun);
    }

    private void OnGameRestart()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);

        GameState = MENU;

        OnGameRun();
    }

    private void OnLoose()
    {
        // create Ender_Or_Restarter menu-box
        OnWinnder(); // <<==|

        GameState = LOSE;

        ChangeAndRunSound(_environmentSound, _clipLoose);
    }

    private void OnGameDone()
    {
        Debug.Log("Application.Quit();");
        Application.Quit();
    }
}
