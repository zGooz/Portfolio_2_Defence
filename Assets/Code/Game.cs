
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


[RequireComponent(typeof(AudioSource))]

public class Game : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject liveBoxObject;
    [SerializeField] private GameObject respawner;

    [SerializeField] private GameObject gameStarterPrefab;
    [SerializeField] private GameObject gameSuspenserPrefab;
    [SerializeField] private GameObject gameRestareterPrefab;

    [SerializeField] private AudioClip gameRun;
    [SerializeField] private AudioClip gameLosing;
    [SerializeField] private AudioClip gameWinner;

    [SerializeField] private AudioSource audioNotification;
    [SerializeField] private AudioSource backgroundMusic;

    private GameObject gameStarter;
    private GameObject gameSuspenser;
    private GameObject gameRestarter;

    public GameObject GameStarter => gameStarter;
    public GameObject GameResumer => gameSuspenser;
    public GameObject GameEnderOrRestarter => gameRestarter;

    private GameStartOrEnd scriptGameStart;
    private GameRestartOrEnd scriptGameEnd;
    private GameResume scriptGamePause;

    private Spawner respawn;
    private Player player;
    private LifeBox liveBox;

    public event UnityAction GameStart;
    public event UnityAction YouWinner;
    public event UnityAction YouLouse;

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

    private void Awake()
    {
        createMenuBox(ref gameStarter, ref gameStarterPrefab, ref scriptGameStart);

        respawn = respawner.GetComponent<Spawner>();
        audioNotification = GetComponent<AudioSource>();
        player = playerObject.GetComponent<Player>();
        liveBox = liveBoxObject.GetComponent<LifeBox>();
    }

    private void OnEnable()
    {
        scriptGameStart.StartGame += OnStartGame;
        scriptGameStart.ExitGame += OnExitGame;
        respawn.Winner += OnWinnder;
        liveBox.Dead += OnLoose;
    }

    private void OnDisable()
    {
        respawn.Winner -= OnWinnder;
        liveBox.Dead -= OnLoose;

        if (scriptGameEnd is GameRestartOrEnd)
        {
            scriptGameEnd.RestartGame -= OnGameRestart;
            scriptGameEnd.ExitGame -= OnExitGame;
        }

        if (scriptGamePause is GameResume)
        {
            scriptGamePause.ResumeGame -= OnGameResume;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameState == GAME)
            {
                GameState = PAUSE;

                createMenuBox(ref gameSuspenser, ref gameSuspenserPrefab, ref scriptGamePause);

                scriptGamePause.ResumeGame += OnGameResume;
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
        Destroy(gameSuspenser);
    }

    private void OnWinnder()
    {
        if (IsNotBeforeLosed())
        {
            GameState = WINNER;
            OnYouWinner();
        }

        OnYouWinner();

        createMenuBox(ref gameRestarter, ref gameRestareterPrefab, ref scriptGameEnd);

        scriptGameEnd.RestartGame += OnGameRestart;
        scriptGameEnd.ExitGame += OnExitGame;

        ChangeAndRunSound(audioNotification, gameWinner);
    }

    private bool IsNotBeforeLosed()
    {
        return GameState != LOSE;
    }

    private void OnStartGame()
    {
        if (GameState == MENU) 
        { 
            OnGameStart(); 
        }

        GameState = GAME;

        scriptGameStart.StartGame -= OnStartGame;
        scriptGameStart.ExitGame -= OnExitGame;

        Destroy(gameStarter);

        ChangeAndRunSound(audioNotification, gameRun);
    }

    private void OnGameRestart()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);

        GameState = MENU;

        OnStartGame();
    }

    private void OnLoose()
    {
        GameState = LOSE;

        // create Ender_Or_Restarter menu-box
        OnWinnder(); 
        OnYouLouse();

        ChangeAndRunSound(audioNotification, gameLosing);
    }

    private void OnExitGame()
    {
        Debug.Log("Application.Quit();");
        Application.Quit();
    }

    private void OnYouWinner()
    {
        YouWinner?.Invoke();
    }

    private void OnYouLouse()
    {
        YouLouse?.Invoke();
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
}
